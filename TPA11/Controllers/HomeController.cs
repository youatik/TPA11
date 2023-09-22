using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using TPA11.Models;
using TPA11.Data; 
using Microsoft.EntityFrameworkCore; 
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPA11.Models;

//added comment


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
