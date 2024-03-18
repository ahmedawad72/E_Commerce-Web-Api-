using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class CartRepo : GenericRepo<Cart>, ICartRepo
    {
        public CartRepo(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Cart?> FindByUserId(Guid Id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.UserId == Id);
        }

        public async Task<bool> CreateCartForUserAsync(Guid userId, IUnitOfWork _unitOfWork)
        {
            var newCart = new Cart
            {
                UserId = userId,
            };

            try
            {
                await _unitOfWork.Carts.AddAsync(newCart);
                await _unitOfWork.SaveAndCommitChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
         
        }
    }
}
