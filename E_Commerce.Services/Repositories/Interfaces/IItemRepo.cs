using E_Commerce.Entities.DbSet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface IItemRepo:IGenericRepo<Item>
    {
      //  Task<byte[]> ConvertImageFormIntoBytesAsync(IFormFile imageFile);
        
    }
}
