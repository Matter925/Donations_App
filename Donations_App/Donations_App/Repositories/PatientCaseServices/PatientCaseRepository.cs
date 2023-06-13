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
            var dateTody= DateTime.Today; 
           
            var AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.IsComplete != true && c.PatientCaseDate.AddDays(c.LimitTime).CompareTo(dateTody) > 0 && c.Rate != 1).ToListAsync();
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
           
            if (page == 1 && limit ==0)
            {
                return new CaseResponse
                {
                    patientCases = AllCases,
                    Pages = page,
                    CurrentPage = page,
                    Count = AllCases.Count()

                };
            }
            var Patients = AllCases;
            var pageResult = limit * (1f);
            var pageCount =Math.Ceiling(Patients.Count() / pageResult);
            
            var cases = AllCases.Skip((page - 1) * (int)pageResult)
            .Take((int)pageResult);
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount,
                Count = Patients.Count()
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
                    Address = dto.Address,  
                    PatientCaseDate= DateTime.Now,
                    UserId = dto.UserId,   
                    LimitTime = dto.LimitTime,  
                    Rate = dto.Rate,
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
            paient.Address = dto.Address;
            paient.LimitTime = dto.LimitTime;
            paient.Rate = dto.Rate;
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
                paient.DonationCount ++;
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

            if (page == 1 && limit == 0)
            {
                AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c=>c.IsComplete == true).ToListAsync();
                return new CaseResponse
                {
                    patientCases = AllCases,
                    Pages = page,
                    CurrentPage = page,
                    Count = AllCases.Count()

                };
            }
            var Patients = await _context.PatientsCases.Where(c => c.IsComplete == true).ToListAsync();
            var pageResult = limit * (1f);
            var pageCount = Math.Ceiling(Patients.Count() / pageResult);
           
            
            var cases = await _context.PatientsCases
            .OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.IsComplete == true).Skip((page - 1) * (int)pageResult)
            .Take((int)pageResult).ToListAsync();
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount,
                Count = Patients.Count()
            };
           


        }

        public async Task<CaseResponse> Search(string query, int page, int limit)
        {
            var results = await _context.PatientsCases.Where(p => p.Name.Contains(query) || p.Description.Contains(query)).ToListAsync();
            if (page == 1 && limit == 0)
            {
                
                return new CaseResponse
                {
                    patientCases = results,
                    Pages = page,
                    CurrentPage = page,
                    Count = results.Count()

                };
            }

                var pageResult = limit * (1f);
                var pageCount = Math.Ceiling(results.Count() / pageResult);
                var cases = await _context.PatientsCases
                .OrderBy(o => o.Name).Include(m => m.Category).Where(p => p.Name.Contains(query) || p.Description.Contains(query)).Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult).ToListAsync();
                return new CaseResponse
                {
                    patientCases = cases,
                    CurrentPage = page,
                    Pages = (int)pageCount,
                    Count = results.Count()
                };
          }

        public async Task<CaseResponse> QuickPatientsCases(int page, int limit)
        {
            var dateTody = DateTime.Today;
            var AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.Rate == 1 && c.PatientCaseDate.AddDays(c.LimitTime).CompareTo(dateTody) > 0).ToListAsync();
            if (page == 1 && limit == 0)
            {
                
                return new CaseResponse
                {
                    patientCases = AllCases,
                    Pages = page,
                    CurrentPage = page,
                    Count = AllCases.Count()

                };
            }
            var Patients = AllCases;
            var pageResult = limit * (1f);
            var pageCount = Math.Ceiling(Patients.Count() / pageResult);

            var cases = AllCases.Skip((page - 1) * (int)pageResult)
            .Take((int)pageResult);
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount,
                Count = Patients.Count()
            };



        }
        public async Task<CaseResponse> Filter(FilterDto dto , int page, int limit)
        {
            var results = _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).ToList();
            var query = results.AsQueryable();
            if (dto.CategoryId != 0)
            {
                results = results.Where(c => c.CategoryId == dto.CategoryId).ToList();
            }
            if(dto.GreaterAmount != 0)
            {
                results = results.Where(c => c.Amount >= dto.GreaterAmount).ToList();
            }
            if (dto.lessAmount != 0)
            {
                results = results.Where(c => c.Amount <= dto.lessAmount).ToList();
            }
            results = results.Where(c => c.IsComplete == dto.IsComplete).ToList();
            if (page == 1 && limit == 0)
            {

                return new CaseResponse
                {
                    patientCases = results,
                    Pages = page,
                    CurrentPage = page,
                    Count = results.Count()

                };
            }

            var pageResult = limit * (1f);
            var pageCount = Math.Ceiling(results.Count() / pageResult);
            var cases =results.Skip((page - 1) * (int)pageResult)
            .Take((int)pageResult).ToList();
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount,
                Count = results.Count()
            };
        }

        public async Task<CaseResponse> GetUserPatientCases(string UserId, int page, int limit)
        {
            var userCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.UserId== UserId).ToListAsync();
            if (page == 1 && limit == 0)
            {
                
                return new CaseResponse
                {
                    patientCases = userCases,
                    Pages = page,
                    CurrentPage = page,
                    Count = userCases.Count()

                };
            }
            var pageResult = limit * (1f);
            var pageCount = Math.Ceiling(userCases.Count() / pageResult);


            var cases = await _context.PatientsCases
            .OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.UserId == UserId).Skip((page - 1) * (int)pageResult)
            .Take((int)pageResult).ToListAsync();
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount,
                Count = userCases.Count()
            };
        }

        public async Task<CaseResponse> ExpirePatientsCases(int page, int limit)
        {
            var dateTody = DateTime.Today;

            var AllCases = await _context.PatientsCases.OrderBy(o => o.Name).Include(m => m.Category).Where(c => c.PatientCaseDate.AddDays(c.LimitTime).CompareTo(dateTody) <= 0).ToListAsync();
            if (page == 1 && limit == 0)
            {
                
                return new CaseResponse
                {
                    patientCases = AllCases,
                    Pages = page,
                    CurrentPage = page,
                    Count = AllCases.Count()

                };
            }
            var Patients = AllCases;
            var pageResult = limit * (1f);
            var pageCount = Math.Ceiling(Patients.Count() / pageResult);

            var cases = AllCases.Skip((page - 1) * (int)pageResult)
            .Take((int)pageResult);
            return new CaseResponse
            {
                patientCases = cases,
                CurrentPage = page,
                Pages = (int)pageCount,
                Count = Patients.Count()
            };


        }

        //public async Task<IEnumerable<PatientCase>> Search(string text)
        //{
        //    text = text.ToLower();
        //    var result = _context.PatientsCases.Where(c=>c.Name.ToLower().Contains(text)||c.Description.ToLower().Contains(text)).ToList();
        //}


    }
}
