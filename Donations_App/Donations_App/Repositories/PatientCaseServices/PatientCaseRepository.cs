using Donations_App.Data;
using Donations_App.Dtos.PatientCaseDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;
using Donations_App.Repositories.FileUploadedServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.PatientCaseServices
{
    public class PatientCaseRepository : IPatientCaseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUploadedService _fileUploadedService;
        public PatientCaseRepository(ApplicationDbContext context , IFileUploadedService fileUploadedService)
        {
            _context = context;
            _fileUploadedService = fileUploadedService;
        }

        public async Task<IEnumerable<PatientCase>> GetAllPatientsCases()
        {
            var cases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).ToListAsync();
            if(cases != null)
            {
                foreach (var item in cases)
                {
                    if (item.AmountPaid == item.Amount)
                    {
                        item.IsComplete = true;
                        _context.PatientsCases.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }    
            
            return cases;
       
        }
        public async Task<PatientCase> GetPatientCaseByID(int id)
        {
            var patientCase = await _context.PatientsCases.Include(c => c.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (patientCase == null)
            {
                return null;
            }
            return patientCase;
        }

        public async Task<IEnumerable<PatientCase>> GetByCategoryId(int categoryId)
        {
            var cases = await _context.PatientsCases.Where(c => c.CategoryId == categoryId).OrderBy(o => o.Name).Include(m => m.Category).ToListAsync();
            return cases;
        }

        public async Task<GeneralRetDto> CreatePatientCase(PatientCaseDto dto)
        {
            var Patient = await _context.PatientsCases.Where(p => p.Name == dto.Name).SingleOrDefaultAsync();
            if (Patient == null)
            {
                var ImagePath = await _fileUploadedService.UploadCaseImagesAsync(dto.Image);
                var patient = new PatientCase
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Amount = dto.Amount,
                   ImageName = ImagePath,
                    CategoryId = dto.CategoryId,    
                };
                await _context.PatientsCases.AddAsync(patient);
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
                Message = "The Patient Case is already exist"
            };
        }

        public async Task<GeneralRetDto> DeletePatientCase(int id)
        {
            var patientCase = await _context.PatientsCases.FindAsync(id);
            if(patientCase == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = $"No Patient Case was found with ID: {id}",
                };
            }
             _context.PatientsCases.Remove(patientCase);
            _context.SaveChanges();
            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Deleted",
            };

        }


        public async Task<GeneralRetDto> UpdatePatientCase(int id,PatientCaseDto dto)
        {
            var paient = await _context.PatientsCases.Include(c=> c.Category).SingleOrDefaultAsync(p=>p.Id == id);
            if (paient == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = $"No Patient Case was found with ID: {id}",
                };
            }
            if(dto.Image !=null)
            {
                var imagePath = await _fileUploadedService.UploadCaseImagesAsync(dto.Image);
                paient.ImageName = imagePath;
            }
            paient.Name = dto.Name;
            paient.Description = dto.Description;
            paient.CategoryId = dto.CategoryId;
            paient.Amount = dto.Amount;
             _context.PatientsCases.Update(paient);
            _context.SaveChanges();
            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Updated",
            };
        }
    }
}
