using E_Commerce.Entities.DbSet;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface ICartRepo:IGenericRepo<Cart>
    {
        Task<Cart?> FindByUserId(string Id);
      
    }
}
