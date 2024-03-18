using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public IAppUserRepo Users { get; }
        public IItemRepo Items {get;}
        public ICategoryRepo Categories { get; }
        public ICartRepo Carts { get; }
        public IOrderRepo Orders { get; }
        public IOrderItemRepo OrderItems { get; }
        public ICategoryItemRepo CategoryItems { get; }
        public ICartItemRepo CartItems { get; }

        public UnitOfWork(AppDbContext context,IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
            Users = new AppUserRepo(_context,_mapper);
            Items = new ItemRepo(_context,_mapper);
            Categories = new CategoryRepo(_context,_mapper);
            Carts = new CartRepo(_context,_mapper);
            Orders = new OrderRepo(_context,_mapper);
            OrderItems = new OrderItemRepo(_context,_mapper);
            CategoryItems = new CategoryItemRepo(_context,_mapper);
            CartItems = new CartItemRepo(_context,_mapper);
            
        }

        public virtual async Task<bool> SaveAndCommitChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }
}
