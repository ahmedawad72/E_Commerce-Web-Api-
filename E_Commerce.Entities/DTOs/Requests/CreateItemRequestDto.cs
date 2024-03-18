using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace E_Commerce.Entities.DTOs.Requests
{
    public class CreateItemRequestDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
        [Required]
        public int Amount { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; } // Use IFormFile for image upload
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Range(0, 1)]
        public decimal Discount { get; set; }
        public IEnumerable<Guid> CategoryIds { get; set; }  
    }

}
