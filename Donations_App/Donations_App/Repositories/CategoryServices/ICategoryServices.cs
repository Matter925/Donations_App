using Donations_App.Models;
using Donations_App.Dtos.CategoryDtos;

namespace Donations_App.Repositories.CategoryServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryByID(int id);
        Task<Category> CreateCategory(CategoryDto Dto);
        Task<Category> UpdateCategory(CategoryDto dto , int id);
        Task<Category> DeleteCategory(int id);
    }
}
