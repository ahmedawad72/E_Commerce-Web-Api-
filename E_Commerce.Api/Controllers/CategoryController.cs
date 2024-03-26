using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using E_Commerce.Entities.DTOs.Responses;
using E_Commerce.Services.Data;
using E_Commerce.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(AppDbContext context, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper=  mapper;
        }


   //     [Authorize(Roles = "User , Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            if (categories == null)
            {
                return NotFound("There is no Categories");
            }
           
            var categoriesDTOs = _mapper.Map<IEnumerable<GetCategoryResponseDto>>(categories);
            return Ok(categoriesDTOs);
        }

   
        
        
    //    [Authorize(Roles = "User , Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById( Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if(category == null)
            {
                return NotFound($"There is no Category with id = {id}");
            }
            var categoryDTO = _mapper.Map<GetCategoryResponseDto>(category);
            return Ok(categoryDTO);
        }



    //    [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(CreateCategoryRequestDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDto);
                var addResult = await _unitOfWork.Categories.AddAsync(category);

                if(!addResult)
                {
                    return BadRequest("Failed to add category");
                }

                var isCategorySaved = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!isCategorySaved)
                {
                    return BadRequest("Failed to save and commit category");
                }
                var categoryResponse = _mapper.Map<CreateCategoryResponseDto>(category);
                return Ok(category);
            }
            return BadRequest("CategoryDTO is Not Valid ");
        }



    //    [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(CreateCategoryRequestDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDto);
                var addResult = _unitOfWork.Categories.Update(category);

                if (!addResult)
                {
                    return BadRequest("Failed to update category");
                }

                var isCategorySaved = await _unitOfWork.SaveAndCommitChangesAsync();
                if (!isCategorySaved)
                {
                    return BadRequest("Failed to save and commit category update");
                }
                var categoryResponse = _mapper.Map<CreateCategoryResponseDto>(category);
                return Ok(category);
            }
            return BadRequest("CategoryDTO is Not Valid ");
        }
     
        
        
        
    //    [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<IActionResult> DeleteCategory(Guid Id)
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
