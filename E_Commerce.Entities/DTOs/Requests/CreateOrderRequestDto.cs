using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Requests
{
    public class CreateOrderRequestDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(250)]
        public string ShipingAddress { get; set; }
        [JsonIgnore]
        public decimal TotalPrice { get; set; }


        public ICollection<Guid> ItemIds { get; set; } 

    }
}
