using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        bool Update(T entity);
    }
}
