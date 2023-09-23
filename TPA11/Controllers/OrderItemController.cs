using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TPA11.Data;
using TPA11.Models;

namespace TPA11.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OrderItems(int orderId)
        {
            // Checher tout les items pour un order ID
            var orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();

            // Passe le orderId à la vue par le ViewBag
            ViewBag.OrderId = orderId;

            return View(orderItems);
        }

    }
}
