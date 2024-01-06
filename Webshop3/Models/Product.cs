using System.ComponentModel.DataAnnotations;

namespace Webshop3.Models
{
    public class Product
    {
        public required int Id { get; set; }

        [StringLength(100)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        public required string ImageUrl { get; set; }
        public List<Customer>? Customers { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
