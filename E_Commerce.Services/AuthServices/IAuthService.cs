using E_Commerce.Entities.AuthenticationModels;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Account;
using E_Commerce.Entities.DTOs.AccountDTOs;

namespace E_Commerce.Services.AuthServices
{
    public interface IAuthService
    {
        public Task<AuthResult> RegisterAsync(ApplicationUser user, string dtoPassword);
        public Task<AuthResult> LoginAsync(ApplicationUser user, string dtoPassword);
        public Task<bool> AddRoleAsync(AddRoleDto roleDto);
        Task<Result> EmailConfirmationAsync(string userId, string code);
        Task<Result> ForgetPassword(string email);
        Task<Result> ResetPasswordAsync(string userId, string newPassword);
    }
}
