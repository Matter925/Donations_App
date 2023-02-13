using Donations_App.Data;
using Donations_App.Dtos.RequestDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;
using Donations_App.Repositories.FileUploadedServices;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.RequestServices
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUploadedService _fileUploadedService;
        public RequestRepository(ApplicationDbContext context, IFileUploadedService fileUploadedService)
        {
            _context = context;
            _fileUploadedService = fileUploadedService;
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

        public async Task<IEnumerable<Request>> GetAllRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetByUserID(string UserId)
        {
            
            var requests = await _context.Requests.Where(o => o.UserId == UserId).ToListAsync();
            return requests;
        }
        public async Task<GeneralRetDto> AccepteRequest(int RequestID)
        {
            var request = await _context.Requests.FindAsync(RequestID);
            if(request != null)
            {
                request.Accepted = true;
                request.Rejected = false;
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
                request.Rejected = true;
                request.Accepted = false;
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
    }
}
