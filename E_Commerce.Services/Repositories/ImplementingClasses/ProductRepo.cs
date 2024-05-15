using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.OrderDTOs;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context=context;
        }

        public async Task<Product> GetProductIncludeCategory(string productId)
        {
            return await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == productId);
        }
       
    }
}
