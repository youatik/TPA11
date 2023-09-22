using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TPA11.Data;
using TPA11.Models;

namespace TPA11.Controllers
{
    public class ClientOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllOrders()
        {
            var clientOrders = await _context.ClientOrders
                .Include(co => co.Client) // Include the Client navigation property
                .ToListAsync();

            return View(clientOrders);
        }
    }
}
