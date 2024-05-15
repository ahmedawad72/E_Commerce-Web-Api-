using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DbSet
{
    public class ProductCategory
    {
        public string? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
