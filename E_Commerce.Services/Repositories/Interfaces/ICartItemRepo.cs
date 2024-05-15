using E_Commerce.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface ICartItemRepo:IGenericRepo<CartItem>
    {
        Task<bool> DeleteItemFromCart( string itemId, string cartId);
        
    }
}
