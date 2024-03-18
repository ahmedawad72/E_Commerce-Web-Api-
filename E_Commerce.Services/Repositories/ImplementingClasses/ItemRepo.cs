using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class ItemRepo:GenericRepo<Item>, IItemRepo
    {
        private readonly AppDbContext _context;
        public ItemRepo(AppDbContext context, IMapper mapper) : base(context,mapper)
        {
        }

        ////public async Task<byte[]> ConvertImageFormIntoBytesAsync(IFormFile imageFile)
        ////{
        ////    if (imageFile == null || imageFile.Length == 0)
        ////        return null;

        ////    using (var memoryStream = new MemoryStream())
        ////    {
        ////        await imageFile.CopyToAsync(memoryStream);
        ////        return memoryStream.ToArray();
        ////    }
        ////}
    }
}
