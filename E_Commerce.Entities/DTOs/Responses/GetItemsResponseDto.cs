using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Responses
{
    public class GetItemsResponseDto
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int Amount { get; set; }
        public byte[] Image { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice => Price * (1 - Discount);
        public ICollection<Guid> CategoriesIds { get; set; }
    }
}
