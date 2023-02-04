using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Donations_App.Dtos.CategoryDtos;
using Donations_App.Repositories.CategoryServices;
using Microsoft.AspNetCore.Authorization;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        //------------------------------------------------------------------
        // Add The EndPoints of The Category Model
        //------------------------------------------------------------------

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var category = await _categoryServices.GetAllCategories();

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateNewCategory")]
        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryServices.CreateCategory(dto);
                if (result == null)
                {
                    return BadRequest("The Category is exist");
                }
                return Ok(result);
               
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var result =await _categoryServices.UpdateCategory(dto,id);
                if(result == null)
                {
                    return NotFound($"No category was found with ID {id} ");
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {

            var result = await _categoryServices.DeleteCategory(id);
            if(result == null)
            {
                return NotFound();  
            }
            return Ok(result);
        }

    }
}
