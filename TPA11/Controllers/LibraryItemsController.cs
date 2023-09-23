using Microsoft.AspNetCore.Mvc;
using System.Data;

using TPA11.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // <-- Ajouté pour le logging
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.Data.SqlClient;
using System.Data.Common; 




namespace TPA11.Controllers
{
    public class LibraryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LibraryItemsController> _logger;  // <-- Declarer un logger

        public LibraryItemsController(ApplicationDbContext context, ILogger<LibraryItemsController> logger)  // <-- Injecter le logger
        {
            _context = context;
            _logger = logger;  // <-- Initialiser le logger
        }

        //trouve tout les livres par procédure stockée
        public IActionResult LibraryItems()
        {
            var libraryItems = _context.LibraryItems
                .FromSqlRaw("call GetLibraryItems")
                .ToList();

            return View(libraryItems);
        }


        public async Task<IActionResult> ViewByISBN(long isbn)
        {
            // Chercher le LibraryItem par ISBN
            var libraryItem = await _context.LibraryItems
                .FirstOrDefaultAsync(li => li.ean_isbn13 == isbn);

            if (libraryItem == null)
            {
                return NotFound(); // Retourne un 404 si non trouvé
            }

            return View("Views/LibraryItems/ViewByISBN.cshtml", libraryItem);
        }



        // Cherche libraryItem pour edit
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

        // Sauvegarde le libraryItem modifié
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("ean_isbn13,Title,Creators,FirstName,LastName,Description,Publisher,PublishDate,Price,Length")] LibraryItem libraryItem)
        {
            _logger.LogInformation($"Received LibraryItem: {System.Text.Json.JsonSerializer.Serialize(libraryItem)}");  // <-- Logger pour debug

            if (id != libraryItem.ean_isbn13)
            {
                

                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
                _logger.LogInformation($"Model state is invalid. Errors: {string.Join(", ", errors)}"); // <-- Logger pour debug
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

        // Retourne la vue pour editer par ISBN
        public IActionResult IsbnEdit()
        {
            return View();
        }

        // POST: pour trouver l'item à editer
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
                    // Gérez l'élément non trouvé.
                    return View("IsbnEdit");
                }
            }
            else
            {
                // Gérez un ISBN invalide.
                return View("IsbnEdit");
            }
        }


        // Mène à la vue qui permet de supprimer un livre
        public IActionResult DeleteByIsbn()
        {
            return View();
        }

        // affiche le message qui determine si le livre peut être effacé et qui va faire le delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(long isbn)
        {
            bool canDelete = await CanDeleteFromLibrary(isbn);
            if (!canDelete)
            {
                // renvoye une vue spécifique qui affiche le message d'erreur
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

        // fonction qui fait appel a la procedure stockée pour déterminer si le livre peut être effacé
        public async Task<bool> CanDeleteFromLibrary(long eanIsbn13) 
        {
            try
            {
                var conn = _context.Database.GetDbConnection(); 
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


        // Retourne la vue pour créer un livre
        public IActionResult Create()
        {
            return View();
        }

        // Crée un livre
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
