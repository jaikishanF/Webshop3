using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop3.Data;
using Webshop3.Models;

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


        //public IActionResult Index()
        //{
        //    var customer = _context.Customer.Single(c => c.Email == _userManager.GetUserName(User));
        //    _context.Entry(customer)
        //        .Collection(c => c.ShoppingCart)
        //        .Load();
        //    ViewData["ShoppingCartItems"] = customer.ShoppingCart;

        //    return View();
        //}

        // view the shopping cart from the logged in customer
        public async Task<IActionResult> Index()
        {
            var userEmail = _userManager.GetUserName(User);
            var customer = await _context.Customer.SingleAsync(c => c.Email == userEmail);

            // Retrieve shopping cart items for this customer
            var shoppingCartItems = await _context.ShoppingCartItems
                                                  .Where(sci => sci.CustomerId == customer.Id)
                                                  .Include(sci => sci.Product) // Include Product details
                                                  .ToListAsync();

            // You can still use ViewData to pass the cart items to the view
            ViewData["ShoppingCartItems"] = shoppingCartItems;

            return View();
        }


        // add the product to the logged in customer shopping cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                var userEmail = _userManager.GetUserName(User);
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == userEmail);

                if (customer == null)
                {
                    return Json(new { success = false, message = "Customer not found." });
                }

                var existingItem = await _context.ShoppingCartItems
                                                 .FirstOrDefaultAsync(sci => sci.CustomerId == customer.Id && sci.ProductId == productId);

                if (existingItem != null)
                {
                    // Increment quantity if item already exists in cart
                    existingItem.Quantity++;
                }
                else
                {
                    // Add new ShoppingCartItem if it doesn't exist
                    var newItem = new ShoppingCartItem
                    {
                        CustomerId = customer.Id,
                        ProductId = productId,
                        Quantity = 1
                    };
                    _context.ShoppingCartItems.Add(newItem);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Product added to cart" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            var userEmail = _userManager.GetUserName(User);
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == userEmail);

            if (customer == null)
            {
                return NotFound(); // Customer not found
            }

            // Find the ShoppingCartItem that needs to be removed
            var cartItem = await _context.ShoppingCartItems
                                         .FirstOrDefaultAsync(sci => sci.CustomerId == customer.Id && sci.ProductId == productId);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return Json(new { productId });
            }

            return NotFound(); // ShoppingCartItem not found or already removed
        }

    }
}