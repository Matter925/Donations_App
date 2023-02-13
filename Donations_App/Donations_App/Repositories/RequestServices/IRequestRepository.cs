using Donations_App.Dtos.RequestDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.RequestServices
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAllRequests();
        Task<IEnumerable<Request>> GetByUserID(string UserId);
        Task<GeneralRetDto> CreateRequest(RequestDto Dto);

        Task<GeneralRetDto> AccepteRequest(int RequestID);
        Task<GeneralRetDto> RejecteRequest(int RequestID);

        //Task<GeneralRetDto> UpdateCategory(CategoryDto dto, int id);
        //Task<GeneralRetDto> DeleteCategory(int id);
    }
}
