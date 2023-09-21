using System;
using Microsoft.AspNetCore.Mvc;
using TPA11.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // <-- Add this for logging
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common; // for DbConnection and DbCommand




namespace TPA11.Controllers
{
    public class LibraryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LibraryItemsController> _logger;  // <-- Declare a logger

        public LibraryItemsController(ApplicationDbContext context, ILogger<LibraryItemsController> logger)  // <-- Inject a logger
        {
            _context = context;
            _logger = logger;  // <-- Initialize logger
        }

        public IActionResult LibraryItems()
        {
            var libraryItems = _context.LibraryItems
                .FromSqlRaw("call GetLibraryItems")
                .ToList();

            return View(libraryItems);
        }

        // GET: LibraryItems/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryItem = await _context.LibraryItems.FindAsync(id);
            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // POST: LibraryItems/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("ean_isbn13,Title,Creators,FirstName,LastName,Description,Publisher,PublishDate,Price,Length")] LibraryItem libraryItem)
        {
            _logger.LogInformation($"Received LibraryItem: {System.Text.Json.JsonSerializer.Serialize(libraryItem)}");  // <-- Log here

            if (id != libraryItem.ean_isbn13)
            {
                

                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
                _logger.LogInformation($"Model state is invalid. Errors: {string.Join(", ", errors)}");
            }

            if (ModelState.IsValid)
            {
               

                try
                {
                    _context.Update(libraryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryItemExists(libraryItem.ean_isbn13))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(LibraryItems));
            }

            return View(libraryItem);
        }

        private bool LibraryItemExists(long id)
        {
            return _context.LibraryItems.Any(e => e.ean_isbn13 == id);
        }

        // GET: LibraryItems/IsbnEdit
        public IActionResult IsbnEdit()
        {
            return View();
        }

        // POST: LibraryItems/FindItemToEdit
        [HttpPost]
        public async Task<IActionResult> FindItemToEdit(string isbn)
        {
            long isbnLong;
            if (long.TryParse(isbn, out isbnLong))
            {
                var libraryItem = await _context.LibraryItems.FindAsync(isbnLong);
                if (libraryItem != null)
                {
                    return RedirectToAction("Edit", new { id = isbnLong });
                }
                else
                {
                    // Handle item not found
                    return View("IsbnEdit");
                }
            }
            else
            {
                // Handle invalid ISBN
                return View("IsbnEdit");
            }
        }


        // GET: LibraryItems/DeleteByIsbn => Mène à la vue qui permet de supprimer un livre
        public IActionResult DeleteByIsbn()
        {
            return View();
        }

        // POST: LibraryItems/DeleteConfirmed => Methode qui affiche le message qui determine si le livre peut être effacé
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(long isbn)
        {
            bool canDelete = await CanDeleteFromLibrary(isbn);
            if (!canDelete)
            {
                // Handle it (e.g., return a specific view that shows the error message)
                return Json(new { message = "It cannot be deleted because it is referenced by an existing order" });
            }

            var libraryItem = await _context.LibraryItems.FindAsync(isbn);
            if (libraryItem != null)
            {
                _context.LibraryItems.Remove(libraryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(LibraryItems));
            }
            return NotFound();
        }




        public async Task<bool> CanDeleteFromLibrary(long eanIsbn13) //method ADO.net et non EF car EF ne fonctionne pas?
        {
            try
            {

                //await using DbConnection conn = _context.Database.GetDbConnection(); //va interrompre la connection EF
                //await conn.OpenAsync();

                var conn = _context.Database.GetDbConnection(); //on ouvre la connection et on ne la ferme jamais? EF s'en occupe
                await conn.OpenAsync();

                await using var command = conn.CreateCommand();
                command.CommandText = "CALL CanDeleteFromLibrary(@ean_isbn13_param)";

                var param = command.CreateParameter();
                param.ParameterName = "@ean_isbn13_param";
                param.Value = eanIsbn13;

                command.Parameters.Add(param);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) == 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                return false;
            }
        }


        // GET: LibraryItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryItems/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ean_isbn13,Title,Creators,FirstName,LastName,Description,Publisher,PublishDate,Price,Length")] LibraryItem libraryItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(LibraryItems));
            }
            return View(libraryItem);
        }






    }
}
