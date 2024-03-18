using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
        public virtual ICollection<Item>? Items { get; set; }
        public virtual ICollection<CategoryItem>? CategoriesItems { get; set; }

    }
}
