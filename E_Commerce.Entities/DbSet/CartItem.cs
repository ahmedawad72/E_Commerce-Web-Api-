using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class CartItem
    {
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }



        public Guid CartId { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }


        public Guid ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}
