using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.ProductDTOs;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [Authorize(Roles = "User , Admin")]
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _unitOfWork.Products.GetAllAsync();  
            var productsDtoList = new List<GetProductDto>();
            foreach (var product in productList)
            {
                var productDto = new GetProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Image = product.Image,
                    CategoryIDs = product.Categories.Select(c => c.Id).ToList(),
                };
                if(productDto != null)
                {
                    productsDtoList.Add(productDto); 
                }

            }
            if (productsDtoList.Any())
            {
               return Ok(productsDtoList);
            }
            return NotFound("There are no Products yet");
        }


        [Authorize(Roles = "User , Admin")]
        [HttpGet]
        [Route("GetProduct{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = new GetProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                CategoryIDs = product.Categories.Select(c => c.Id).ToList(),
            };

            return Ok(productDto);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] CreateProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                // Handling Image
                byte[] fileByteArray = new byte[] { };    
                if (productDto.Image != null)
                {
                    using (var item = new MemoryStream())
                    {
                        productDto.Image.CopyTo(item);
                        fileByteArray = item.ToArray();
                    }
                }
                // Mapping UpdateDTO to Model
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Image = fileByteArray,
                    Price = productDto.Price,
                    Quantity = productDto.Quantity
                };
                // Update Product Categories
                foreach (var id in productDto.CategoryIDs)
                {
                    var category = await _unitOfWork.Categories.GetByIdAsync(id);
                    product.Categories.Add(category);
                }
                // Add and Save Changes To Database 
                var result = await _unitOfWork.Products.AddAsync(product);
                if (result == false)
                {
                    return BadRequest("failed to add ,the item is null");
                }
                var save = await _unitOfWork.SaveAndCommitChangesAsync();
            }
            return Ok(productDto);
               
        }


        [Authorize(Roles = "Admin")]
        [Route("Update/{Id}")]
        [HttpPut]
        public async Task<IActionResult> Update(string productId, [FromForm] UpdateProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product =await _unitOfWork.Products.GetProductIncludeCategory(productId);
                if (product == null)
                {
                    return NotFound("the requested product for updating is not found");
                }
                // Handling Image
                using var stream = new MemoryStream();
                await productDto.Image.CopyToAsync(stream);
                
                // Mapping UpdateDTO to Model
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Image = stream.ToArray();
                product.Price = productDto.Price;
                product.Quantity = productDto.Quantity;
                
                // Update Product Categories
                product.Categories = new List<Category> { };
                foreach (var categoryId in productDto.CategoryIDs)
                {
                    var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                    product.Categories.Add(category);
                }
                // Update Database
                var isUpdated = _unitOfWork.Products.Update(product);
                if (isUpdated == false)
                {
                    return BadRequest("failed to update the item");
                }
                var saveCategoryItem = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!saveCategoryItem)
                {
                    return BadRequest("failed to Save And Commit the item updates ");
                }

                return Ok(productDto);
            }

            return BadRequest("The Model State is not Valid");
        }


        [Authorize(Roles = "Admin")]
        [Route("Delete{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var IsDeleted = await _unitOfWork.Products.DeleteAsync(id);
            if (IsDeleted == false)
            {
                return BadRequest("failed to Delete Item ");
            }
            var commitDelete = await _unitOfWork.SaveAndCommitChangesAsync();
            if (commitDelete == false)
            {
                return BadRequest("failed to Save And Commit  Item deletion");
            }

            return Ok("item deleted successfully");
        }
       
    }
}
