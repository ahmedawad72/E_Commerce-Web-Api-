using E_Commerce.Entities.DbSet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Category_Item 

            modelBuilder.Entity<CategoryItem>()
                .HasKey(ci => new { ci.CategoryId, ci.ItemId });

            modelBuilder.Entity<Item>().
                HasMany(i => i.Categories).
                WithMany(c => c.Items).
                UsingEntity<CategoryItem>();


            modelBuilder.Entity<CategoryItem>().    
                 HasOne(ci => ci.Category)
                .WithMany(c => c.CategoriesItems)
                .HasForeignKey(ci => ci.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // This will prevent category from being deleted when CategoryItem exist

            modelBuilder.Entity<CategoryItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.CategoriesItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict);  // This will prevent Item from being deleted when CategoryItem is deleted
            #endregion

            #region Cart_Item

            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartId, ci.ItemId });

            modelBuilder.Entity<Item>().
                HasMany(i => i.Carts).
                WithMany(c => c.Items).
                UsingEntity<CartItem>();


            modelBuilder.Entity<CartItem>().
                 HasOne(ci => ci.Cart)
                .WithMany(c => c.CartsItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Restrict); // This will prevent Cart from being deleted when CartItem exist

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.CartsItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // This will prevent Item from being deleted when CartItem is exist
            #endregion  
            
            #region Order_Item

            modelBuilder.Entity<OrderItem>()
                .HasKey(ci => new { ci.OrderId, ci.ItemId });

            modelBuilder.Entity<Item>().
                HasMany(i => i.Orders).
                WithMany(o => o.Items).
                UsingEntity<OrderItem>();


            modelBuilder.Entity<OrderItem>().
                 HasOne(ci => ci.Order)
                .WithMany(c => c.OrdersItems)
                .HasForeignKey(ci => ci.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // This will prevent Order from being deleted when OrderItem exist


            modelBuilder.Entity<OrderItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.OrdersItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // This will prevent Item from being deleted when OrderItem is deleted


            #endregion
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Item> Items { get; set; }
    
    }
}
