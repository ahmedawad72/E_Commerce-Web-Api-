using E_Commerce.Entities.AuthenticationModels;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using E_Commerce.Services.Configuration;
using E_Commerce.Entities.DTOs.Responses;
using System.Data;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using E_Commerce.Services.Repositories.Interfaces;

namespace E_Commerce.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<AuthResult> RegisterAsync( ApplicationUser appUser, string password)
        {
                #region Check for email and phone number region

                // check if the email is already registered
                var emailExistingUser = await _userManager.FindByEmailAsync(appUser.Email);
                if (emailExistingUser != null)
                {
                        return AuthResult.Failed(
                                 new List<string> {"This email has already registered"} );
                }
                // check if the Phone Number is already registered
                var phoneExistingUser = await _userManager.FindByPhoneNumberAsync(appUser.PhoneNumber);
                if (phoneExistingUser != null)
                {
                      return AuthResult.Failed(
                              new List<string> { "This Phone number has already registered" } );
                }

                #endregion

                #region Creating user region
           
                var is_created = await _userManager.CreateAsync(appUser, password);
                if (is_created.Succeeded)
                {
                    // Generate Token
                    await _userManager.AddToRoleAsync(appUser, "User");
                    var jwtSecurityToken = await GenerateJwtToken(appUser);

                    // Additional step: Create a cart for the user
                    //var cartCreationResult = await _unitOfWork.Carts.CreateCartForUserAsync(appUser.Id,_unitOfWork);
                    //if (!cartCreationResult)
                    //{
                    //    return AuthResult.Failed(new List<string> { "Failed to create a cart for the user" });
                    //}
                
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    var roles = new List<string> { "User" };
                    var expiryDate = jwtSecurityToken.ValidTo;

                    return  AuthResult.Successful(token, expiryDate, roles);

                }
            #endregion
            // User creation not succeeded
            return AuthResult.Failed(is_created.Errors.Select(e => e.Description).ToList());
        }
      
        public async Task<AuthResult> LoginAsync(ApplicationUser appUser,string password)
        {
            
                // check if the email is not  registered 
                var existingUser = await _userManager.FindByEmailAsync(appUser.Email);
                var isValidPassword = await _userManager.CheckPasswordAsync(existingUser, password);
                if (existingUser == null || !isValidPassword)
                {
                         return AuthResult.Failed(
                                 new List<string>{  "Invalid email or password !"} );
                }
                
                // Generate Token
                var jwtSecurityToken = await GenerateJwtToken(existingUser);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                var roles = await _userManager.GetRolesAsync(existingUser);
                var expiryDate = jwtSecurityToken.ValidTo;

                return AuthResult.Successful(token, expiryDate, roles); 
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
    }  
}

