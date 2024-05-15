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
        public OrderItem()
        {
            TotalPrice = UnitPrice * Quantity;
        }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice;

        public string OrderId { get; set; }
        public virtual Order Order { get; set; }    
       
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
       

    }
}
