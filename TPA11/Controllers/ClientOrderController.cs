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
                .Include(co => co.Client) 
                .ToListAsync();

            return View(clientOrders);
        }

        //recherche par ISBN dans le commandes 
        public async Task<IActionResult> SearchByIsbn(long? ean_isbn13)
        {
            if (!ean_isbn13.HasValue)
            {
                return View(new List<ClientOrder>());
            }

            var ordersWithItem = await _context.OrderItems
                .Where(oi => oi.ean_isbn13 == ean_isbn13)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Client)
                .Select(oi => oi.Order)
                .Distinct() // In case an item appears multiple times.
                .ToListAsync();

            return View(ordersWithItem);
        }

    }
}
