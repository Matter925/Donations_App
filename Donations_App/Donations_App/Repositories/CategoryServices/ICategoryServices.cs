using Donations_App.Models;
using Donations_App.Dtos.CategoryDtos;
using Donations_App.Dtos.ReturnDto;

namespace Donations_App.Repositories.CategoryServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryByID(int id);
        Task<GeneralRetDto> CreateCategory(CategoryDto Dto);
        Task<GeneralRetDto> UpdateCategory(CategoryDto dto , int id);
        Task<GeneralRetDto> DeleteCategory(int id);
    }
}
