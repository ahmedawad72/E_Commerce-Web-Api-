using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.ProductDTOs;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        
        [Authorize(Roles = "User , Admin")]
        [HttpGet("GetCartProducts")]
        public async Task<IActionResult> GetProducts()
        {
            string? username = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (username == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByNameAsync(username);
           
            var cart = await _unitOfWork.Carts.FindByUserId(user.Id);
            if (cart == null)
            {
                var Result = await _unitOfWork.Carts.AddAsync(new Cart { UserId = user.Id });
                if (!Result)
                {
                    return BadRequest();
                }
            }
            var cartProductsList = cart.Items;

            if (cartProductsList == null)
            {
                return NotFound();
            }
            var cartProductDtosList = new List<GetProductDto>();
            foreach (var product in cartProductsList)
            {
                var cartProductDto = new GetProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Image = product.Image,

                };
                cartProductDtosList.Add(cartProductDto);
            }
            return Ok(cartProductDtosList);
        }


        [Authorize(Roles = "User , Admin")]
        [HttpPost("AddProductToCart/{Id}")]
        public async Task<IActionResult> AddProductToCart(string productId)
        {
            string? username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null)
            {
                return Unauthorized();
            }
            ApplicationUser? user = await _userManager.FindByNameAsync(username);

            Cart? cart = await _unitOfWork.Carts.FindByUserId(user.Id);
            if (cart == null)
            {
                var Result = await _unitOfWork.Carts.AddAsync(new Cart { UserId = user.Id });
                if (!Result)
                {
                    return BadRequest();
                }
            }

            var product =await _unitOfWork.Products.GetByIdAsync(productId);
            if(product ==null || product.Quantity==0)
            {
                return NotFound("this item is not avilable now ");
            }

            cart.Items.Add(product);
            cart.TotalPrice += product.Price;

            var isChangesSaved = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!isChangesSaved)
            {
                return BadRequest();
            }
            return Ok();
        }


        [Authorize(Roles = "User , Admin")]
        [HttpPost("RemoveProductFromCart/{Id}")]
        public async Task<IActionResult> RemoveItemFromCart(string itemId)
        {
            string? username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null)
            {
                return Unauthorized();
            }

            ApplicationUser? user = await _userManager.FindByNameAsync(username);

            Cart? Cart = await _unitOfWork.Carts.FindByUserId(user.Id);
            if (Cart == null)
            {
                return BadRequest();
            }

            var item = await _unitOfWork.Products.GetByIdAsync(itemId);
            if (item == null)
            {
                return BadRequest("The Item With the requested id Doesn't Exists!");
            }
            Cart.Items.Remove(item);

            var isChangesSaved = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!isChangesSaved)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
