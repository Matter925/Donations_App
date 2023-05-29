using Donations_App.Data;
using Donations_App.Dtos.RequestDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;
using Donations_App.Repositories.FileUploadedServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.RequestServices
{
    public class RequestRepository : IRequestRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IFileUploadedService _fileUploadedService;
        public RequestRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IFileUploadedService fileUploadedService)
        {
            _context = context;
            _fileUploadedService = fileUploadedService;
            _userManager = userManager;
        }

        

        public async Task<GeneralRetDto> CreateRequest(RequestDto dto)
        {
            var request = await _context.Requests.Where(c => c.FullName == dto.FullName && c.UserId == dto.UserId).FirstOrDefaultAsync();
            if (request == null)
            {
                var PathID = await _fileUploadedService.UploadRequestFileID(dto.ID_Photo);
                var PathReport = await _fileUploadedService.UploadRequestFileReport(dto.Medical_Report);
                var newreq = new Request
                {
                    FullName = dto.FullName,
                    Address = dto.Address,
                    Age = dto.Age,
                    Phone = dto.Phone,
                    Description_Request =dto.Description_Request,
                    ID_Photo = PathID,
                    RequestStatus = "wait",
                    Medical_Report = PathReport,
                    UserId = dto.UserId,
                };
                await _context.Requests.AddAsync(newreq);
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
                Message = "The Request is already exist"
            };
        }

        public async Task<IEnumerable<UserRequestsDto>> GetAllRequests()
        {

            var userReq = await _userManager.Users.Include(r => r.Requests).Select(u => new UserRequestsDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.PhoneNumber,
                UserId = u.Id,
                Requests = u.Requests.ToList(),
               Count = u.Requests.Count()

            }).ToListAsync();

            return userReq;

        }

        public async Task<ResRequest> GetByUserID(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return new ResRequest
                {
                    Success=false,

                };
            }
            var requests = await _context.Requests.Where(o => o.UserId == UserId).ToListAsync();
            return new ResRequest
            {
                Success =true,
                Requests = requests,
                Count = requests.Count()
            };
        }
        public async Task<GeneralRetDto> AccepteRequest(int RequestID)
        {
            var request = await _context.Requests.FindAsync(RequestID);
            if(request != null)
            {
                request.RequestStatus = "accepted";
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return new GeneralRetDto
                {
                    Success = true,
                    Message = "Successfully Accepted"

                };
            }
            return new GeneralRetDto
            {
                Success = false,
                Message = "Request Id  not found !!"
            };
        }

        public async Task<GeneralRetDto> RejecteRequest(int RequestID)
        {
            var request = await _context.Requests.FindAsync(RequestID);
            if (request != null)
            {
                request.RequestStatus = "rejected";
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return new GeneralRetDto
                {
                    Success = true,
                    Message = "Successfully Rejected"

                };
            }
            return new GeneralRetDto
            {
                Success = false,
                Message = "Request Id  not found !!"
            };
        }

        public async Task<GeneralRetDto> DeleteRequest(int RequestId)
        {
            var request = await _context.Requests.FindAsync(RequestId);
            if (request == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = $"No request was found with ID: {RequestId}",
                };
            }
            _context.Remove(request);
            _context.SaveChanges();
            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Deleted"
            };
        }

        public async Task<Request> GetRequestByID(int id)
        {
            var result = await _context.Requests.SingleOrDefaultAsync(c => c.Id == id);
            return result;
        }

        public async Task<ResRequest> GetAcceptedRequests(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return new ResRequest
                {
                    Success = false,

                };
            }
            var requests = await _context.Requests.Where(o => o.UserId == UserId && o.RequestStatus== "accepted").ToListAsync();
            return new ResRequest
            {
                Success = true,
                Requests = requests
            };
        }

        public async Task<IEnumerable<UserRequestsDto>> GetWaitRequests()
        {

            var userReq = await _userManager.Users.Include(r => r.Requests).Where(d => d.Requests.Any(x=>x.RequestStatus== "wait")).Select(u => new UserRequestsDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.PhoneNumber,
                UserId = u.Id,
                Requests = u.Requests.Where(s=>s.RequestStatus== "wait").ToList(),
                Count = u.Requests.Where(s => s.RequestStatus == "wait").Count()

            }).ToListAsync();

            return userReq;
        }
    }
}
