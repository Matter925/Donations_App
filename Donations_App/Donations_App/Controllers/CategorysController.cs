using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Donations_App.Services.CategoryServices;
using Donations_App.Dtos.CategoryDtos;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategorysController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        //------------------------------------------------------------------
        // Add The EndPoints of The Category Model
        //------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GettAllCategory()
        {
            var category = await _categoryServices.GetAllCategories();

            return Ok(category);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryServices.CreateCategory(dto);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("The Category is exist");
            }
            return BadRequest(ModelState);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryServices.GetCategoryByID(id);

                if (category != null)
                {
                    category.Name = dto.Name;
                    category.Description = dto.Description;
                    _categoryServices.UpdateCategory(category);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryServices.DeleteCategory(id);
            return Ok(result);
        }

    }
}
