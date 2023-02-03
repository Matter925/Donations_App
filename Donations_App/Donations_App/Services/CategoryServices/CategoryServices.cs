using Donations_App.Models;
using Microsoft.EntityFrameworkCore;
using Donations_App.Dtos.CategoryDtos;
using Donations_App.Data;

namespace Donations_App.Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;
        public CategoryServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateCategory(CategoryDto dto)
        {
            var category = await _context.Categories.Where(c => c.Name == dto.Name).FirstOrDefaultAsync();
            if (category == null)
            {
                var cat = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description,
                };
                await _context.Categories.AddAsync(cat);
                _context.SaveChanges();
                return category;
            }

            return null;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var categoty = await _context.Categories.FindAsync(id);
            _context.Remove(categoty);
            _context.SaveChanges();
            return categoty;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByID(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();

            return category;
        }
    }
}
