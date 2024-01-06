using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop3.Data;

namespace Webshop3.Controllers
{
    public class ShopController : Controller
    {
        private readonly Webshop3Context _context;

        public ShopController(Webshop3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Product.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> ViewProduct(int id)
        {
            var product = await _context.Product.SingleAsync(p => p.Id == id);
            return View(product);
        }
    }
}
