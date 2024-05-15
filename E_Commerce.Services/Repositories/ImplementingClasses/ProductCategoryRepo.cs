using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class ProductCategoryRepo : GenericRepo<ProductCategory>, IProductCategoryRepo
    {
        public ProductCategoryRepo(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
   
        }
    }
}
