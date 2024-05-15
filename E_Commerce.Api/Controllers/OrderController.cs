using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.OrderDTOs;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            if (orders == null)
            {
                return NotFound();
            }
            var orderDTOs = orders.Select(orderDto => new GetOrderDto
            {
                Id = orderDto.Id,
                TotalPrice = orderDto.TotalPrice,
                CreatedOn = orderDto.CreatedOn,
                UserId = orderDto.UserId
            });
            return Ok(orderDTOs);
        }

  
        [Authorize(Roles = "User , Admin")]
        [HttpGet("GetOrder/{Id}")]
        public async Task<IActionResult> GetOrder(string Id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(Id);
            if (order == null)
            {
                return NotFound();
            }
            var orderDto = new GetOrderDto
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                IsCreated = order.IsCreated,
                CreatedOn = order.CreatedOn,
                UserId = order.UserId
            };
            return Ok(orderDto);
        }

        [Authorize(Roles = "User , Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Get User Data
            string username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null)
            {
                return Unauthorized();
            }
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            //Order Processing
            var order = new Order();
            order.Items = user.Cart.Items;
            order.TotalPrice = user.Cart.TotalPrice;
            order.ShipingAddress = user.Address;
            order.IsCreated = true;
            order.UserId = user.Id;

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteOrder(string Id)
        {
            var isIOrderDeleted = await _unitOfWork.Orders.DeleteAsync(Id);
            if (!isIOrderDeleted)
            {
                return BadRequest();
            }
            var isChangesSaved = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!isChangesSaved)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
