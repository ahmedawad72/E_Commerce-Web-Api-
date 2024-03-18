using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.ImplementingClasses
{
    public class AppUserRepo : GenericRepo<ApplicationUser>, IAppUserRepo
    {
        public AppUserRepo(AppDbContext context,IMapper mapper) : base(context,mapper)
        {

        }
 
    }
}
