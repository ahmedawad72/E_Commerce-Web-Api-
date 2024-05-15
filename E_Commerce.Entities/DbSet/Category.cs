using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DbSet
{
    public class Category
    {
        public Category()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
       
        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        
        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
  
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

    }
}
