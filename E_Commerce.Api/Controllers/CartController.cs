using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Responses;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{ 
      [Route("api/[controller]")]
      [ApiController] 
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(AppDbContext context, IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [Authorize(Roles = "User , Admin")]
        //user
        [HttpGet("GetCartProducts")]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("You must be logged in to perform this action.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized("You must be logged in to perform this action.");
            }
            var cart = await _unitOfWork.Carts.FindByUserId(user.Id);
            if (cart == null)
            {
                return NotFound("No cart found for the current user.");
            }
    
            var CartItems = await _unitOfWork.CartItems.GetAllAsync();

            if (CartItems == null)
            {
                return NotFound("Cart Items not Found");
            }

            var itemsList = new List<Item>();
            foreach (var cartItem in CartItems)
            {
                var item = await _unitOfWork.Items.GetByIdAsync(cartItem.ItemId);
                if (item != null)
                { 
                    itemsList.Add(item);
                }
            }
            var itemsDtoList = _mapper.Map<IEnumerable<GetItemsResponseDto>>(itemsList);
            return Ok(itemsDtoList);
        }


        [Authorize(Roles = "User , Admin")]
        [HttpDelete("item/{itemId}")]
        public async Task<IActionResult> DeleteItemFromCart(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("You must be logged in to perform this action.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            var cart = await _unitOfWork.Carts.FindByUserId(user.Id);
            if (cart == null)
            {
                return NotFound("No cart found for the current user.");
            }

            var result = await _unitOfWork.CartItems.DeleteItemFromCart(user.Id, cart.Id);
            if (!result)
            {
                return NotFound($"item not found");
            }
            bool saveSuccessful = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!saveSuccessful)
            {
                return BadRequest("Could not remove item from the cart. Please try again.");
            }

            return NoContent(); 
    }
}
}
