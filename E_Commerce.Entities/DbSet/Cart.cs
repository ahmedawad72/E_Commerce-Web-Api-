using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class Cart
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public decimal TotalPrice{ get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Product> Items { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
