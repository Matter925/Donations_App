using Donations_App.Data;
using Donations_App.Dtos.PatientCaseDtos;
using Donations_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.PatientCaseServices
{
    public class PatientCaseRepository : IPatientCaseRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientCaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PatientCase> CreatePatientCase(PatientCaseDto dto)
        {
            var Patient = await _context.PatientsCases.Where(p => p.Name == dto.Name).SingleOrDefaultAsync();
            if (Patient == null)
            {
                using var dataStream = new MemoryStream();
                await dto.Image.CopyToAsync(dataStream);
                var patient = new PatientCase
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Amount = dto.Amount,
                    Image = dataStream.ToArray(),
                    CategoryId = dto.CategoryId,    
                };
                await _context.PatientsCases.AddAsync(patient);
                 _context.SaveChanges();
                return patient;
                
            }
            return null;
        }

        public async Task<PatientCase> DeletePatientCase(int id)
        {
            var patientCase = await _context.PatientsCases.FindAsync(id);
            if(patientCase == null)
            {
                return null;
            }
             _context.PatientsCases.Remove(patientCase);
            _context.SaveChanges();
            return patientCase;
            
        }

        public async Task<IEnumerable<PatientCase>> GetAllPatientsCases()
        {
            var cases = await _context.PatientsCases.OrderBy(o=> o.Name).Include(m=> m.Category).ToListAsync();
            return cases;
        }

        public async Task<IEnumerable<PatientCase>> GetByCategoryId(int categoryId)
        {
            var cases = await _context.PatientsCases.Where(c => c.CategoryId == categoryId).OrderBy(o => o.Name).Include(m => m.Category).ToListAsync();
            return cases;
        }

        public async Task<PatientCase> GetPatientCaseByID(int id)
        {
            var patientCase = await _context.PatientsCases.Include(c=>c.Category).SingleOrDefaultAsync(m=>m.Id ==id);  
            if (patientCase == null)
            {
                return null;
            }
            return patientCase;
        }

        public async Task<PatientCase> UpdatePatientCase(int id,PatientCaseDto dto)
        {
            var paient = await _context.PatientsCases.Include(c=> c.Category).SingleOrDefaultAsync(p=>p.Id == id);
            if (paient == null)
            {
                return null;
            }
            if(dto.Image !=null)
            {
                using var dataStream = new MemoryStream();
                await dto.Image.CopyToAsync(dataStream);
                paient.Image = dataStream.ToArray();
            }
            paient.Name = dto.Name;
            paient.Description = dto.Description;
            paient.CategoryId = dto.CategoryId;
            paient.Amount = dto.Amount;
             _context.PatientsCases.Update(paient);
            _context.SaveChanges();
            return paient;
        }
    }
}
