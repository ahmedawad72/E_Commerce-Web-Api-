using AutoMapper;
using E_Commerce.Entities.AuthenticationModels;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Account;
using E_Commerce.Entities.DTOs.AccountDTOs;
using E_Commerce.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<AuthResult> RegisterAsync(ApplicationUser appUser, string password)
        {
           // check if the email is already registered
            var emailExistingUser = await _userManager.FindByEmailAsync(appUser.Email);
            if (emailExistingUser != null)
            {
                return AuthResult.Failed(
                         new List<string> { "This email has already registered" });
            }
            // check if the Phone Number is already registered
            var phoneExistingUser = await _userManager.FindByPhoneNumberAsync(appUser.PhoneNumber);
            if (phoneExistingUser != null)
            {
                return AuthResult.Failed(
                        new List<string> { "This Phone number has already registered" });
            }


            var is_created = await _userManager.CreateAsync(appUser, password);
            if (is_created.Succeeded)
            {
                // Generate Token
                await _userManager.AddToRoleAsync(appUser, "User");
                var jwtSecurityToken = await GenerateJwtToken(appUser);

                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                var roles = new List<string> { "User" };
                var expiryDate = jwtSecurityToken.ValidTo;

                return AuthResult.Successful(token, expiryDate, roles);

            }
            // User creation not succeeded
            return AuthResult.Failed(is_created.Errors.Select(e => e.Description).ToList());
        }

        public async Task<AuthResult> LoginAsync(ApplicationUser appUser, string password)
        {

            // check if the email is not  registered 
            var existingUser = await _userManager.FindByEmailAsync(appUser.Email);
            var isValidPassword = await _userManager.CheckPasswordAsync(existingUser, password);
            if (existingUser == null || !isValidPassword)
            {
                return AuthResult.Failed(
                        new List<string> { "Invalid email or password !" });
            }

            // Generate Token
            var jwtSecurityToken = await GenerateJwtToken(existingUser);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var roles = await _userManager.GetRolesAsync(existingUser);
            var expiryDate = jwtSecurityToken.ValidTo;

            return AuthResult.Successful(token, expiryDate, roles);
        }

        public async Task<bool> AddRoleAsync(AddRoleDto roleDto)
        {
            var user = await _userManager.FindByIdAsync(roleDto.UserId.ToString());
            var roleExists = await _roleManager.RoleExistsAsync(roleDto.Role);
            if (user == null || !roleExists)
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, roleDto.Role))
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleDto.Role);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            // Retrieve claims and roles for the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            // Create a list of claims and add standard claims
            var claims = new List<Claim>
            {
               new Claim("Id", user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Sub, user.Email),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:SecretKey"]);
            var expiryDate = DateTime.UtcNow.AddDays(7);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken
            (
               issuer: _configuration["JwtConfig:Issuer"],
               audience: _configuration["JwtConfig:Audience"],
               expires: expiryDate,
               claims: claims,
               signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }

        public async Task<Result> EmailConfirmationAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new Result { Message = "Invalid Email" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Result { Message = $"Unable to load user with ID '{userId}'." };
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var isConfirmed = await _userManager.ConfirmEmailAsync(user, code);

            var result = new Result();
            result.Message = isConfirmed.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            result.IsSuccesed = isConfirmed.Succeeded;

            return result;
        }

        public async Task<Result> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new Result { Message = "Email Is Not Found" };
            }
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new Result { Message = "No User With This Email" };
            }

            if (!user.EmailConfirmed)
            {
                return new Result { Message = "Please Confirm Email" };
            }

            return new Result
            {
                Message = user.Id,
                IsSuccesed = true
            };
        }

        public async Task<Result> ResetPasswordAsync(string userId, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPassword))
            {
                return new Result { Message = "Password or UserId cannot be Empty or Null" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Result { Message = $"Unable to load user with ID '{userId}'." };
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var isReset = await _userManager.ResetPasswordAsync(user, code, newPassword);

            var result = new Result();
            result.Message = isReset.Succeeded ? "Password Reset Succesfully" : "Unable to reset password";
            result.IsSuccesed = isReset.Succeeded;

            return result;
        }
    }
}

