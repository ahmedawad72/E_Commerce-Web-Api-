using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Requests
{
    public class CreateCategoryRequestDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        [Required, MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
    }
}
