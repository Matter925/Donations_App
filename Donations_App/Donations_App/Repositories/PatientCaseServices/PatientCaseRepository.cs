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

        public async Task<CaseResponse> GetAllPatientsCases(int page  , int limit)
        {
            var AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).ToListAsync();
           if(AllCases != null)
            {
                foreach (var item in AllCases)
                {
                    if (item.AmountPaid == item.Amount)
                    {
                        item.IsComplete = true;
                        _context.PatientsCases.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            if (page == 0 && limit ==0)
            {
                 AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.IsComplete != true).ToListAsync();
                return new CaseResponse
                {
                    patientCases = AllCases,

                };
            }
            var pageResult = limit * (1f);
            var pageCount =Math.Ceiling(_context.PatientsCases.Count() / pageResult);
            var cases = await _context.PatientsCases
                .Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult)
                .OrderBy(o => o.Name).Include(m => m.Category).Where(c=>c.IsComplete != true).ToListAsync();
            return new CaseResponse{
                patientCases = cases,
                CurrentPage = page,
                Pages =(int)pageCount
            };
       
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
            var paient = await _context.PatientsCases.FindAsync(id);
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

        public async Task<GeneralRetDto> IncreamentAmountPaid(int CartId)
        {
            var items = await _context.CartItems.Where(c=>c.CartId== CartId).ToListAsync();
            if (items == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = "",
                };
            }
            foreach(var item in items)
            {
                var paient =await _context.PatientsCases.FindAsync(item.PatientCaseId);
                paient.AmountPaid += item.setAmount;
                _context.PatientsCases.Update(paient);
                _context.SaveChanges();
            }
            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Increament",
            };
        }

        public async Task<CaseResponse> CompletedPatientsCases(int page, int limit)
        {
            var AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).ToListAsync();
            if (AllCases != null)
            {
                foreach (var item in AllCases)
                {
                    if (item.AmountPaid == item.Amount)
                    {
                        item.IsComplete = true;
                        _context.PatientsCases.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            if (page == 0 && limit == 0)
            {
                AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c=>c.IsComplete == true).ToListAsync();
                return new CaseResponse
                {
                    patientCases = AllCases,

                };
            }
            var pageResult = limit * (1f);
            var pageCount = Math.Ceiling(_context.PatientsCases.Count() / pageResult);
            var cases = await _context.PatientsCases
                .Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult)
                .OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.IsComplete == true).ToListAsync();
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount
            };

        }

        //public async Task<IEnumerable<PatientCase>> Search(string text)
        //{
        //    text = text.ToLower();
        //    var result = _context.PatientsCases.Where(c=>c.Name.ToLower().Contains(text)||c.Description.ToLower().Contains(text)).ToList();
        //}


    }
}
