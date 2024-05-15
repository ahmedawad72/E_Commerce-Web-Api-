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
        public CartItem()
        {
            this.TotalPrice = this.UnitPrice * Quantity;
            //Cart = new Cart();
            //Product = new Product();
            //CartId = Cart.Id;
            //ProductId = Product.Id;
        }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }
    
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
