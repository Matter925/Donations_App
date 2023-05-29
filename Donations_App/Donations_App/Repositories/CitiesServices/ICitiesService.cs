using Donations_App.Models;

namespace Donations_App.Repositories.CitiesServices
{
    public interface ICitiesService
    {
        Task<IEnumerable<Governorate>> GetGovernorates();
        Task<IEnumerable<City>> GetCities(int GovernorateId);

    }
}
