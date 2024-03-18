using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; set; } = String.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;

        [Required, MaxLength(250)]
        public string ShipingAddress { get; set; }=String.Empty;

        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Item>? Items { get; set; }
        public virtual ICollection<OrderItem>? OrdersItems { get; set; }

    }
}
