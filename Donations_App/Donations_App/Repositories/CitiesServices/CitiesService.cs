using Donations_App.Data;
using Donations_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.CitiesServices
{
    public class CitiesService : ICitiesService
    {
        private readonly ApplicationDbContext _context;
        
        public CitiesService(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<IEnumerable<City>> GetCities(int GovernorateId)
        {
            var Cities = await _context.Cities.Where(c=>c.governorateId == GovernorateId).ToListAsync();
            return Cities;
        }

        public async Task<IEnumerable<Governorate>> GetGovernorates()
        {
            var Gov = await _context.Governorates.ToListAsync();
            return Gov;
        }
    }
}
