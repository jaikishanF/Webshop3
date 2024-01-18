using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop3.Data;
using X.PagedList;

namespace Webshop3.Controllers
{
    public class ShopController : Controller
    {
        private readonly Webshop3Context _context;

        public ShopController(Webshop3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 6; // 6 items per page
            var pageNumber = page ?? 1;

            var products = await _context.Product.ToPagedListAsync(pageNumber, pageSize);
            return View(products);
        }

        public async Task<IActionResult> ViewProduct(int id)
        {
            var product = await _context.Product.SingleAsync(p => p.Id == id);
            return View(product);
        }
    }
}
