using Donations_App.Dtos.RequestDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.RequestServices
{
    public interface IRequestRepository
    {
        Task<IEnumerable<UserRequestsDto>> GetAllRequests();
        Task<IEnumerable<Request>> GetByUserID(string UserId);
        Task<GeneralRetDto> CreateRequest(RequestDto Dto);

        Task<GeneralRetDto> AccepteRequest(int RequestID);
        Task<GeneralRetDto> RejecteRequest(int RequestID);

        Task<Request> GetRequestByID(int id);
        Task<GeneralRetDto> DeleteRequest(int RequestId);
    }
}
