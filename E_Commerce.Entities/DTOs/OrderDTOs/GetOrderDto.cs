using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.OrderDTOs
{
    public class GetOrderDto
    {
        public string Id { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCreated { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }
}
