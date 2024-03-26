using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using E_Commerce.Entities.DTOs.Responses;
using E_Commerce.Services.AuthServices;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ItemController(AppDbContext context, IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


      //  [Authorize(Roles = "User , Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
           var items = await  _unitOfWork.Items.GetAllAsync();

            if (items.Any())
            {
                 var ItemsResponseDto = _mapper.Map<IEnumerable<GetItemsResponseDto>>(items); 
                 return Ok(ItemsResponseDto);
            }

                return NotFound("There are no items yet");
        }


   //     [Authorize(Roles = "User , Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById( Guid id)
        {
           var item = await  _unitOfWork.Items.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            
            var ItemResponseDto = _mapper.Map<GetItemsResponseDto>(item);
          
            return Ok(ItemResponseDto);
        }
        
        
      //[Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Create([FromForm]CreateItemRequestDto itemDto)
        {
            if (ModelState.IsValid)
            {
                // Mapping 
                var item = _mapper.Map<Item>(itemDto);

                #region Add Item
                var result = await _unitOfWork.Items.AddAsync(item);
                if (result == false)
                {
                    return BadRequest("failed to add ,the item is null");
                }
                
                #endregion

                #region Adding Item to its Categories

                foreach (var categoryId in itemDto.CategoryIds)
                {
                    var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            //        item.Categories.Add(category);
                    category.Items.Add(item);


                    //var categoryItem = new CategoryItem
                    //{
                    //    CategoryId = categoryId,
                    //    ItemId = item.Id,
                    //    Category = category,
                    //    Item = item
                    //};

                    //var res = await _unitOfWork.CategoryItems.AddAsync(categoryItem);
                    //if (!res)
                    //{
                    //    return BadRequest("failed to add ,the categoryItem is null");
                    //}


                }
                #endregion

                var saveCategoryItem = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!saveCategoryItem)
                {
                    return BadRequest("failed to Save And Commit CategoryItem ");
                }
                var response = _mapper.Map<CreateItemResponseDto>(item);
                return Ok(response);
            }
            return BadRequest("the CreateItemRequestDto model state is not valid");
        }


       // [Authorize(Roles = "Admin")]
        [Route("Update")]
        [HttpPut]

        public async Task<IActionResult> Update([FromForm] UpdateItemRequestDto itemDto)
        {
            if (ModelState.IsValid)
            {
                var item = _mapper.Map<Item>(itemDto);


                #region Updating item

                    var result =  _unitOfWork.Items.Update(item);
                    if (result == false)
                    {
                        return BadRequest("failed to update ,the item is null");
                    }
                
                #endregion
                
                
                #region update Item  Categories

                    foreach (var categoryId in itemDto.CategoryIds)
                    {
                        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);

                        var categoryItem = new CategoryItem
                        {
                            CategoryId = categoryId,
                            ItemId = item.Id,
                            Category = category,
                            Item = item
                        };
                        var res = await _unitOfWork.CategoryItems.AddAsync(categoryItem);
                        if (!res)
                        {
                            return BadRequest("failed to add ,the categoryItem is null");
                        }
                    }
                #endregion 

               var saveCategoryItem = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!saveCategoryItem)
                {
                    return BadRequest("failed to Save And Commit CategoryItem ");
                }
                return Ok(itemDto);
            }

            return BadRequest("The Model State is InValid");
        }




       // [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
           
               var IsDeleted =  await _unitOfWork.Items.DeleteAsync(id);
                if (IsDeleted == false)
                {
                    return BadRequest("failed to Delete Item ");
                }
                var commitDelete = await _unitOfWork.SaveAndCommitChangesAsync();
                if (commitDelete == false )
                {
                    return BadRequest($"failed to Save And Commit  Item deletion {commitDelete}");
                }

                return Ok("delete succes");
        }


   //     [Authorize(Roles = "User , Admin")]
        [HttpPost("AddItemToCart/{Id}")]
        public async Task<IActionResult> AddItemToCart(Guid Id)
        {
            string? Username = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (Username == null)
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(Username);
   

            var Cart = await _unitOfWork.Carts.FindByUserId(user.Id);
            if (Cart == null)
            {
                var Result = await _unitOfWork.Carts.AddAsync(new Cart { UserId = user.Id });

                if (!Result)
                    return BadRequest();

                Result = await _unitOfWork.SaveAndCommitChangesAsync();

                if (!Result)
                    return BadRequest();
            }
            var item = await _unitOfWork.Items.GetByIdAsync(Id);
            if (item == null)
                return BadRequest($"The Item With Id {Id} Doesn't Exists!");

            var result = await _unitOfWork.CartItems.AddAsync(new CartItem
            {
                CartId = Cart.Id,
                ItemId = Id,
            });

            if (!result)
                return BadRequest();
            result = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
