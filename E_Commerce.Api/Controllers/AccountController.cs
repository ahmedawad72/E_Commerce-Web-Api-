using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using E_Commerce.Services.AuthServices;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.ImplementingClasses;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     //   private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public AccountController(AppDbContext context, IUnitOfWork unitOfWork,
                                 IAuthService authService,IMapper mapper)
        {
       //     _context = context;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<ApplicationUser>(requestDto);

                var authResult = await _authService.RegisterAsync(appUser,requestDto.password);
                if (authResult.IsAuthenticated)
                {
                    return Ok(authResult);
                }
                return BadRequest(authResult);
            }
            return BadRequest(requestDto);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<ApplicationUser>(requestDto);
                var authResult = await _authService.LoginAsync(appUser, requestDto.Password);
                if (authResult.IsAuthenticated)
                {
                    return Ok(authResult);
                }
                return BadRequest(authResult);
            }
            return BadRequest(requestDto);
        }


        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddAccountRoleRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.AddRoleAsync(requestDto);

                if (!result)
                {
                    return BadRequest("Failed to add the role to the Account");
                }

                return Ok(requestDto);
            }

           return BadRequest(ModelState);
        }
    }
}
