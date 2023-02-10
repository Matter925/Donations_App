using Donations_App.Models;
using Microsoft.EntityFrameworkCore;
using Donations_App.Dtos.CategoryDtos;
using Donations_App.Data;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Repositories.FileUploadedServices;

namespace Donations_App.Repositories.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUploadedService _fileUploadedService;
        public CategoryServices(ApplicationDbContext context , IFileUploadedService fileUploadedService)
        {
            _context = context;
            _fileUploadedService = fileUploadedService;
        }

        public async Task<GeneralRetDto> CreateCategory(CategoryDto dto)
        {
            var category = await _context.Categories.Where(c => c.Name == dto.Name).FirstOrDefaultAsync();
            if (category == null)
            {
                var ImagePath = await _fileUploadedService.UploadCategoryImagesAsync(dto.Image);
                var cat = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageName = ImagePath,
                };
                await _context.Categories.AddAsync(cat);
                _context.SaveChanges();
                return new GeneralRetDto
                {
                    Success = true,
                    Message = "Successfully"
                };
              }
                return new GeneralRetDto
            {
                Success = false,
                Message = "The Category is already exist"
            };
        }

        public async Task<GeneralRetDto> DeleteCategory(int id)
        {
            var categoty = await _context.Categories.FindAsync(id);
            if(categoty == null)
            {
                return new GeneralRetDto
                {
                    Success = false,    
                    Message = $"No category was found with ID: {id}",
                };
            }
            _context.Remove(categoty);
            _context.SaveChanges();
            return new GeneralRetDto
            {
                Success=true,
                Message="Successfully Deleted"
            };
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {

            var categories= await _context.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategoryByID(int id)
        {
            var result = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            return result;

        }

        public async Task<GeneralRetDto> UpdateCategory(CategoryDto dto , int id)
        {
            
            var catategory= await _context.Categories.FindAsync(id);
            if(catategory == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = $"No category was found with ID: {id}",
                };
            }
            if (dto.Image != null)
            {
                var imagePath = await _fileUploadedService.UploadCategoryImagesAsync(dto.Image);
                catategory.ImageName = imagePath;
            }
            catategory.Name = dto.Name;
            catategory.Description = dto.Description;
            _context.Categories.Update(catategory);
            _context.SaveChanges(true);

            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Updated",
            };
        }
    }
}
