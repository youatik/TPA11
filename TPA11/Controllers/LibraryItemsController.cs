using System;
using Microsoft.AspNetCore.Mvc;
using TPA11.Data;
using Microsoft.EntityFrameworkCore;


namespace TPA11.Controllers
{
    
    namespace TPA11.Controllers
    {
        public class LibraryItemsController : Controller
        {
            private readonly ApplicationDbContext _context;

            public LibraryItemsController(ApplicationDbContext context)
            {
                _context = context;
            }

            public IActionResult LibraryItems()
            {
                var libraryItems = _context.LibraryItems
                    .FromSqlRaw("call GetLibraryItems")
                    .ToList();

                return View(libraryItems);
            }
           
        }
    }

}

