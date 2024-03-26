using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webshop3.Data;
using Webshop3.Models;

namespace Webshop3.Controllers
{
    // Manage all product items
    public class ProductsController : Controller
    {
        // Setup access to the database
        private readonly Webshop3Context _context;

        public ProductsController(Webshop3Context context)
        {
            _context = context;
        }

        // Show all product items
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // View a specific product item
        public async Task<IActionResult> Details(int? id)
        {
            // check if product id exists
            if (id == null)
            {
                return NotFound();
            }

            // Product id exists, now find it
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);

            // If product item is empty
            if (product == null)
            {
                return NotFound();
            }

            // Send product item to the view
            return View(product);
        }

        // Create a new product item: fill in the information
        public IActionResult Create()
        {
            return View();
        }

        // Create a new product item: here the item is sent to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Category,Price,Quantity,ImageUrl")] Product product)
        {
            // If object of type Product is valid. see Product class
            if (ModelState.IsValid)
            {
                // Send product to the database and save it
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Edit specific product item: edit details
        public async Task<IActionResult> Edit(int? id)
        {
            // If product id is empty
            if (id == null)
            {
                return NotFound();
            }
            // Search for product in the database
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Send product item to the view
            return View(product);
        }

        // Edit specific product item: save changes to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Category,Price,Quantity,ImageUrl")] Product product)
        {
            // Check if product id is not same as given in url
            if (id != product.Id)
            {
                return NotFound();
            }

            // If object of type Product is valid. see Product class
            if (ModelState.IsValid)
            {
                try
                {
                    // Send product update to the database and save it
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                // check if data is modified when send to db. This should not be the case, alert malware
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Delete product item: select it
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if product id is real
            if (id == null)
            {
                return NotFound();
            }

            // Product id exists, now find it
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Send product item to the view
            return View(product);
        }

        // Delete product item: item gets deleted here
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get product in database
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                // Delete product if found
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Check method if product is real
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
