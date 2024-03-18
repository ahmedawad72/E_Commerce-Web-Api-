using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Responses
{
    public class GetOrderResponseDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(250)]
        public string ShippingAddress { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public List<Guid> ItemIds { get; set; }
    }
}
