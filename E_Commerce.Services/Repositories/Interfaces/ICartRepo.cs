using E_Commerce.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface ICartRepo:IGenericRepo<Cart>
    {
        Task<Cart?> FindByUserId(Guid Id);
        Task<bool> CreateCartForUserAsync(Guid userId, IUnitOfWork _unitOfWork);


    }
}
