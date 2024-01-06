using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webshop3.Models;

namespace Webshop3.Data
{
    public class Webshop3Context : IdentityDbContext
    {
        public Webshop3Context (DbContextOptions<Webshop3Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set default values for Customer model
            builder.Entity<Customer>()
                .Property(c => c.FirstName)
                .HasDefaultValue("First name");
            builder.Entity<Customer>()
                .Property(c => c.LastName)
                .HasDefaultValue("Last name");
            builder.Entity<Customer>()
                .Property(c => c.Address)
                .HasDefaultValue("Default address");

            // join table customer and product for shoppingcart
            builder.Entity<Customer>()
                .HasMany(c => c.ShoppingCart)
                .WithMany(p => p.Customers)
                .UsingEntity(j => j.ToTable("CustomerProducts"));

            builder.Entity<ShoppingCartItem>()
            .HasKey(sci => new { sci.CustomerId, sci.ProductId });

            builder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Customer)
                .WithMany(c => c.ShoppingCartItems)
                .HasForeignKey(sci => sci.CustomerId);

            builder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(sci => sci.ProductId);
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
