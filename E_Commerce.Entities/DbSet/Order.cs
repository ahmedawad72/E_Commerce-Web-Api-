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
        public Order()
        {
            DateTime CreatedOn = DateTime.Now;
            DateTime UpdatedOn = DateTime.Now;
        }
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
      
        [Required, MaxLength(250)]
        public string ShipingAddress { get; set; }=String.Empty;
        
        public decimal TotalPrice {  get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;
  
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public bool IsCreated { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }      
        
        public virtual ICollection<Product> Items{ get; set; }     
        public virtual ICollection<OrderItem> OrderItems{ get; set; }     
    }
}
