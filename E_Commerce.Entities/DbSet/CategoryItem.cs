using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class CategoryItem
    {
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        
        public Guid ItemId { get; set; }
 
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}
