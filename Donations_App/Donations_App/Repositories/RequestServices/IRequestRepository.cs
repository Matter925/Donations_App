using Donations_App.Dtos.RequestDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.RequestServices
{
    public interface IRequestRepository
    {
        Task<IEnumerable<UserRequestsDto>> GetAllRequests();
        Task<ResRequest> GetByUserID(string UserId);
        Task<ResRequest> GetAcceptedRequests(string UserId);
        Task<IEnumerable<UserRequestsDto>> GetWaitRequests();
        Task<GeneralRetDto> CreateRequest(RequestDto Dto);

        Task<GeneralRetDto> AccepteRequest(int RequestID);
        Task<GeneralRetDto> RejecteRequest(int RequestID);

        Task<Request> GetRequestByID(int id);
        Task<GeneralRetDto> DeleteRequest(int RequestId);
    }
}
