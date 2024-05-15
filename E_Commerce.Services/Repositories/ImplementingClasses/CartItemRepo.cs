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
    public class CartItemRepo : GenericRepo<CartItem>, ICartItemRepo
    {
        public CartItemRepo(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<bool> DeleteItemFromCart( string itemId, string cartId)
        {
            var item = await _dbSet.FindAsync(itemId, cartId);
            if (item == null)
            {
                return false;
            }
            _dbSet.Remove(item);
            return true;
        }

    }
}
