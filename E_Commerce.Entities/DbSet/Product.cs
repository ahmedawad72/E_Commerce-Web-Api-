using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DbSet
{
    public class Product
    {
        public Product()
        {
            Categories = new List<Category>();
            Carts = new List<Cart>();
            Orders = new List<Order>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }


        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        
        [Required]
        public byte[] Image { get; set; }

        public virtual ICollection<Order> Orders{ get; set; }
        public virtual ICollection<Cart> Carts{ get; set; }
        public virtual ICollection<Category> Categories{ get; set; }
        
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

       
    }
}
