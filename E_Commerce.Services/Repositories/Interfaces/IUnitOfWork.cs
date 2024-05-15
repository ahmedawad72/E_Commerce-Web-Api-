using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IAppUserRepo Users { get; }
        IProductRepo Products { get; }
        ICategoryRepo Categories {  get; }
        ICartRepo Carts { get; } 
        IOrderRepo Orders { get; }
        IOrderItemRepo OrderItems { get; }
        IProductCategoryRepo ProductCategories { get; }
        ICartItemRepo CartItems { get; }
        Task<bool> SaveAndCommitChangesAsync();
    }
}
