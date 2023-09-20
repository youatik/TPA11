using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPA11.Models;
using TPA11.Data; // Include the namespace for ApplicationDbContext
using Microsoft.EntityFrameworkCore; // Include the namespace for Entity Framework Core

namespace TPA11.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LibraryItems()
        {
            var libraryItems = _context.LibraryItems
                .FromSqlRaw("call GetLibraryItems")
                .ToList();

            return View(libraryItems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
