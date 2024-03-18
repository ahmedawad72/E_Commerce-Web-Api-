using E_Commerce.Entities.DbSet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services.Configuration
{
        // Extention Methods for UserManager
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByPhoneNumberAsync(this UserManager<ApplicationUser> userManager, string phoneNumber)
        {
            return await userManager?.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
