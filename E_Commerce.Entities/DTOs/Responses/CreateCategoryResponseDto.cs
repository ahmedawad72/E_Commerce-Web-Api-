using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Responses
{
    public class CreateCategoryResponseDto
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
