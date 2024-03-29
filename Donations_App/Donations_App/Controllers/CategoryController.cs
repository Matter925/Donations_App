﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Donations_App.Dtos.CategoryDtos;
using Donations_App.Repositories.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Donations_App.Data;
using Donations_App.Dtos;
using Donations_App.Models;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ApplicationDbContext _context;
        private new List<string> _allowedExtenstions = new List<string> { ".jpeg", ".webp", ".svg" };
        public CategoryController(ICategoryServices categoryServices , ApplicationDbContext context)
        {
            _categoryServices = categoryServices;
            _context = context;
        }

        //------------------------------------------------------------------
        // Add The EndPoints of The Category Model
        //------------------------------------------------------------------
       
        [HttpGet("GetAllCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategory()
        {
            var category = await _categoryServices.GetAllCategories();

            return Ok(category);
        }
        

        [HttpGet("GetCategoryByID/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryByID(int id)
        {
            var category = await _categoryServices.GetCategoryByID(id);
            if(category == null)
            {
                return NotFound($"No category was found with ID: {id}");
            }

            return Ok(category);
        }


        [HttpPost("CreateNewCategory")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    return BadRequest("Only .webp and .jpeg images are allowed!");
                var result = await _categoryServices.CreateCategory(dto);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
               
            }
            return BadRequest(ModelState);
        }

        
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                if(!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    return BadRequest("Only .webp and .jpeg images are allowed!");
                var result =await _categoryServices.UpdateCategory(dto,id);
                if(result.Success)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest(ModelState);
        }

        
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryServices.DeleteCategory(id);
            if(result.Success)
            {
                return Ok(result) ;  
            }
            return NotFound(result);
        }

    }
}
