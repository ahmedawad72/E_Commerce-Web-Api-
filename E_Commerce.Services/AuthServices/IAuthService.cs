using E_Commerce.Entities.AuthenticationModels;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using E_Commerce.Entities.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.AuthServices
{
    public interface IAuthService
    {
        public  Task<AuthResult> RegisterAsync(ApplicationUser user,string dtoPassword);
        public Task<AuthResult> LoginAsync(ApplicationUser user, string dtoPassword);

    }
}
