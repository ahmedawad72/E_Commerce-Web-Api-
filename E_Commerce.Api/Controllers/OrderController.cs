using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using E_Commerce.Entities.DTOs.Responses;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AppDbContext context, IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "User , Admin")]
        [HttpGet("GetOrder/{Id}")]
        public async Task<IActionResult> GetOrderById(Guid Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("You must be logged in to perform this action.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return Unauthorized("You must be logged in to perform this action.");
            }

            var order = await _unitOfWork.Orders.GetByIdAsync(Id);
            if (order == null)
            {
                return NotFound("Order not Found");
            }
            var orderResponse = _mapper.Map<GetOrderResponseDto>(order);
            return Ok(orderResponse);
        }

        [Authorize(Roles = "User , Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
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

            var orders = await _unitOfWork.Orders.GetAllAsync();
            if (orders == null)
            {
                return NotFound("Order not Found");
            }
            var ordersResponse = _mapper.Map<IEnumerable<GetOrderResponseDto>>(orders);
            return Ok(ordersResponse);
        }

        
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto orderRequest)
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
            decimal totalPrice = 0;
            var items= new List<Item>();
            foreach (var itemId in orderRequest.ItemIds)
            {
                var item = await _unitOfWork.Items.GetByIdAsync(itemId);

                if (item != null)
                {
                    totalPrice += item.FinalPrice;
                }
                items.Add(item);
            }
            orderRequest.TotalPrice = totalPrice;

            var order = _mapper.Map<Order>(orderRequest);

            var result = await _unitOfWork.Orders.AddAsync(order);
            if (!result)
            {
                return BadRequest($"Failed to Create Order {order.Name}");
            }
            var saveCategoryItem = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!saveCategoryItem)
            {
                return BadRequest("failed to Save And Commit Order ");
            }

            var orderResponse = _mapper.Map<GetOrderResponseDto>(order);
            return Ok(orderResponse);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteOrder(Guid Id)
        {
            var Result = await _unitOfWork.Orders.DeleteAsync(Id);
            if (!Result)
                return NotFound("Order Not found");
            Result = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!Result)
                return BadRequest("Failed To Commit Order Deletion");
            return NoContent();
        }

    }
}
