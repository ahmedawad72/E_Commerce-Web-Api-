using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class OrderItem
    {
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }    
        public Guid ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }


    }
}
