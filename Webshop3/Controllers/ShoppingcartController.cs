using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop3.Data;

namespace Webshop3.Controllers
{
    public class ShoppingcartController : Controller
    {
        private readonly Webshop3Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingcartController(Webshop3Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // view the shopping cart from the logged in customer
        public IActionResult Index()
        {
            var customer = _context.Customer.Single(c => c.Email == _userManager.GetUserName(User));
            _context.Entry(customer)
                .Collection(c => c.ShoppingCart)
                .Load();
            ViewData["ShoppingCartItems"] = customer.ShoppingCart;

            return View();
        }

        // add the product to the logged in customer shopping cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            // Find the customer and the product
            var customer = await _context.Customer
                                         .Include(c => c.ShoppingCart)
                                         .FirstOrDefaultAsync(c => c.Email == _userManager.GetUserName(User));
            var product = await _context.Product
                                        .FindAsync(productId);

            if (customer == null || product == null)
            {
                return NotFound(); // Or handle the error as appropriate
            }

            customer.ShoppingCart.Add(product);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            var customer = await _context.Customer
                                          .Include(c => c.ShoppingCart)
                                          .FirstOrDefaultAsync(c => c.Email == _userManager.GetUserName(User));

            var product = customer.ShoppingCart.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                customer.ShoppingCart.Remove(product);
                await _context.SaveChangesAsync();
                return Json(new { productId });
            }

            return NotFound(); // or handle the error as appropriate
        }
    }
}