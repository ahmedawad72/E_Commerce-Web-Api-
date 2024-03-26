using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class Item
    {
        public Item()
        {
            Categories = new List<Category>();
            Orders = new List<Order>();
            Carts = new List<Cart>();
            CategoriesItems = new List<CategoryItem>();
            OrdersItems = new List<OrderItem>();
            CartsItems = new List<CartItem>();
        }


        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
        [Required]
        public int Amount { get; set; }

        [Required]
        public byte[] Image { get; set; }
        [Required]
        public decimal Discount { get; set; }
        [Required]
        public decimal Price { get; set; }
        [NotMapped]
        public decimal FinalPrice => Price * (1 - Discount);
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<OrderItem> OrdersItems { get; set; }
        public virtual ICollection<CartItem> CartsItems { get; set; }
        public virtual ICollection<CategoryItem> CategoriesItems { get; set; }

       
    }
}
