using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTOs.CategoryDTOs
{
    public class GetCategoryDto
    {
        public string Id {  get; set; } 
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
