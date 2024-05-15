using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.OrderDTOs;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface IProductRepo:IGenericRepo<Product>
    {
        Task<Product> GetProductIncludeCategory(string productId);
     
    }
}
