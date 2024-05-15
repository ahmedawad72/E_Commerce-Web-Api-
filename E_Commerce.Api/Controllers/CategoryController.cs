using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.CategoryDTOs;
using E_Commerce.Entities.DTOs.ProductDTOs;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [Authorize(Roles = "User , Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            if (categories == null)
            {
                return NotFound("There is no Categories");
            }
            var categoryDtoList = new List<GetCategoryDto>();
            foreach (var category in categories)
            {
                var categoryDto = new GetCategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                };
                if (categoryDto != null)
                {
                    categoryDtoList.Add(categoryDto);
                }
            }
            if (categoryDtoList.Any())
            {
                return Ok();
            }
            else
            {
                return NotFound("NO Categories Found ");
            }
        }


        [Authorize(Roles = "User , Admin")]
        [Route("GetCategoryProducts{Id}")]
        [HttpGet]
        public async Task<IActionResult> GetCategoryProducts(string categoryId)
        {  
            if(categoryId == null)
            {
                return BadRequest();
            }
            var productList =await _unitOfWork.Categories.GetCategoryProducts(categoryId);
            if (!productList.Any())
            {
                return NotFound();
            }
            var productDtoList = new List<GetProductDto>();
            foreach (var product in productList)
            {
                var productDto = new GetProductDto
                {
                    Id = product.Id,    
                    Name = product.Name,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Image = product.Image,
                };
                productDtoList.Add(productDto);
            }
            return Ok(productDtoList);
        }



        [Authorize(Roles = "User , Admin")]
        [Route("GetCategory/{Id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = new GetCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                };
                var isCategoryAdded = await _unitOfWork.Categories.AddAsync(category);

                if (!isCategoryAdded)
                {
                    return BadRequest("Failed to add category");
                }

                var isCategorySaved = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!isCategorySaved)
                {
                    return BadRequest("Failed to save and commit category");
                }

                return Ok(categoryDto);
            }
            return BadRequest("CategoryDTO is Not Valid ");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update/{Id}")]
        public async Task<IActionResult> Update(string categoryId, GetCategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                category.Id = categoryDto.Id;
                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description;
               
                var isCategoryUpdated = _unitOfWork.Categories.Update(category);
                if (!isCategoryUpdated)
                {
                    return BadRequest("Failed to update category");
                }

                var isCategorySaved = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!isCategorySaved)
                {
                    return BadRequest("Failed to save and commit category update");
                }
                return Ok(categoryDto);
            }
            return BadRequest("CategoryDTO is Not Valid ");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteCategory(string Id)
        {
            var Result = await _unitOfWork.Categories.DeleteAsync(Id);
            if (!Result)
                return BadRequest();
            Result = await _unitOfWork.SaveAndCommitChangesAsync();
            if (!Result)
                return BadRequest();
            return Ok();
        }

    }
}
