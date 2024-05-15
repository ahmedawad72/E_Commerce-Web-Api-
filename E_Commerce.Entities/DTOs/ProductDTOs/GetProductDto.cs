using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTOs.ProductDTOs
{
    public class GetProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }
        public List<string>? CategoryIDs { get; set; }
    }
}
