using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPA11.Models;
using TPA11.Data; // Include the namespace for ApplicationDbContext
using Microsoft.EntityFrameworkCore; // Include the namespace for Entity Framework Core
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPA11.Models;

//comment


namespace TPA11.Controllers
{
    
    namespace TPA11.Controllers
    {
        public class HomeController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Privacy()
            {
                return View();
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }

}
