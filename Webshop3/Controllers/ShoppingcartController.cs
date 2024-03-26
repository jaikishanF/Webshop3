using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop3.Data;
using Webshop3.Models;

namespace Webshop3.Controllers
{
    // Handle shopping cart logic for the user
    public class ShoppingcartController : Controller
    {
        private readonly Webshop3Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingcartController(Webshop3Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // View shopping cart for the user logged in
        public async Task<IActionResult> Index()
        {
            // Get logged in customer by user email
            var userEmail = _userManager.GetUserName(User);
            var customer = await _context.Customer.SingleAsync(c => c.Email == userEmail);

            // Retrieve shopping cart items for this customer
            var shoppingCartItems = await _context.ShoppingCartItems
                                                  .Where(sci => sci.CustomerId == customer.Id)
                                                  .Include(sci => sci.Product) // Include Product details
                                                  .ToListAsync();

            // Pass the cart items to the view
            ViewData["ShoppingCartItems"] = shoppingCartItems;

            return View();
        }


        // Add the product to the logged in customer shopping cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                // Get logged in customer by user email
                var userEmail = _userManager.GetUserName(User);
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == userEmail);

                // Check if customer is not logged in
                if (customer == null)
                {
                    return Json(new { success = false, message = "Customer not found." });
                }

                // Get shopping cart data for this user and check if the added product already exists in the cart
                var existingItem = await _context.ShoppingCartItems
                                                 .FirstOrDefaultAsync(sci => sci.CustomerId == customer.Id && sci.ProductId == productId);

                // If product is already in cart
                if (existingItem != null)
                {
                    // Increment quantity 
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
                // Display message for user
                return Json(new { success = true, message = "Product added to cart" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Delete the product from the shopping cart
        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            // Get logged in customer by user email
            var userEmail = _userManager.GetUserName(User);
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == userEmail);

            if (customer == null)
            {
                return NotFound(); // Customer not found
            }

            // Find the ShoppingCartItem that needs to be removed
            var cartItem = await _context.ShoppingCartItems
                                         .FirstOrDefaultAsync(sci => sci.CustomerId == customer.Id && sci.ProductId == productId);

            // if ShoppingCartItem exists, delete it from cart
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