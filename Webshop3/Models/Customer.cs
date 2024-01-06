using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Webshop3.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Shopping cart")]
        public List<Product>? ShoppingCart { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
