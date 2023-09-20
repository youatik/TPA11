using System;
using Microsoft.AspNetCore.Mvc;
using TPA11.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // <-- Add this for logging
using System.Threading.Tasks;
using System.Linq;

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
    }
}
