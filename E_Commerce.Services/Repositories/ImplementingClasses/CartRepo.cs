using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class CartRepo : GenericRepo<Cart>, ICartRepo
    {
        public CartRepo(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Cart?> FindByUserId(string Id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.UserId == Id);
        }

    }
}
