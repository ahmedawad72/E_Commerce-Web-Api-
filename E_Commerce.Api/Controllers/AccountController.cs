using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Account;
using E_Commerce.Entities.DTOs.AccountDTOs;
using E_Commerce.Services.AuthServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //   private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;

        public AccountController(IAuthService authService, IMapper mapper, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _mapper = mapper;
            _emailSender = emailSender;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            var user = new ApplicationUser
            {
                Email = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                UserName = registrationDto.Email,
                PhoneNumber = registrationDto.PhoneNumber
                    
            };
            var authResult = await _authService.RegisterAsync(user,registrationDto.Password);
            if (!authResult.IsAuthenticated)
            {
                return BadRequest(authResult);
            }
            var callbackUrl = await GenerateConfirmEmailUrl(registrationDto.Email);
            var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            await _emailSender.SendEmailAsync(registrationDto.Email, "Confirm your email",
                $"Please confirm your account by <a href='{encodedUrl}'>clicking here</a>.");

            return Ok("Please confirm your account");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationDto model)
        {
            var result = await _authService.EmailConfirmationAsync(model.UserId, model.Code);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userManager.FindByEmailAsync(model.Email) == null)
            {
                return BadRequest("No User With This Email");
            }

            var callbackUrl = await GenerateConfirmEmailUrl(model.Email);
            var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"Please confirm your account by <a href='{encodedUrl}'>clicking here</a>.");
            return Ok("Check Your E-mail");
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgetPassword(model.Email);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            string userId = result.Message;
            var code = GenerateOTP();

            Response.Cookies.Append("UserId", userId, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            Response.Cookies.Append("Code", code, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                $"Code To Reset Password {code}.");

            return Ok("Check Your Email To Reset Password");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var userId = Request.Cookies["UserId"];

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid User Id");
            }

            var result = await _authService.ResetPasswordAsync(userId, model.newPassword);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = loginDto.Email
                };
                var authResult = await _authService.LoginAsync(user, loginDto.Password);
                if (authResult.IsAuthenticated)
                {
                    return Ok(authResult);
                }
                return BadRequest(authResult);
            }
            return BadRequest(loginDto);
        }

       
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("You Logout Succesfully");
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.AddRoleAsync(roleDto);

                if (!result)
                {
                    return BadRequest("Failed to add the role to the Account");
                }

                return Ok(roleDto);
            }

            return BadRequest(ModelState);
        }

        private async Task<string> GenerateConfirmEmailUrl(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Request.Scheme + "://" + Request.Host +
                                    Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
            return callbackUrl;
        }

        private string GenerateOTP()
        {
            var random = new Random();
            var code = random.Next(0, 1000000).ToString();
            return code;
        }
    }
}
