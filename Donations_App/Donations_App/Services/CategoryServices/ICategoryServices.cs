using Donations_App.Models;
using Donations_App.Dtos.CategoryDtos;

namespace Donations_App.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryByID(int id);
        Task<Category> CreateCategory(CategoryDto DTO);
        Task<Category> UpdateCategory(Category category);
        Task<Category> DeleteCategory(int id);
    }
}
