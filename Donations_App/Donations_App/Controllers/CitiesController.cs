using Donations_App.Data;
using Donations_App.Dtos;
using Donations_App.Models;
using Donations_App.Repositories.CitiesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;
        private readonly ApplicationDbContext _context;

        public CitiesController(ICitiesService citiesService , ApplicationDbContext context)
        {
            _citiesService = citiesService;
            _context = context;
        }
        [HttpGet("GetAllGovernorates")]
       
        public async Task<IActionResult> GetAllGovernorates()
        {
            var Governorate = await _citiesService.GetGovernorates();

            return Ok(Governorate);
        }
        [HttpGet("GetCities/{governorateId}")]

        public async Task<IActionResult> GetCities(int governorateId)
        {
            var Cities = await _citiesService.GetCities(governorateId);

            return Ok(Cities);
        }

        //[HttpPost("AddCities")]

        //public async Task<IActionResult> GetAll(GonDto dto)
        //{
        //    var cat = new City
        //    {
        //        Name = dto.Name,
        //        governorateId = dto.governorateId,  

        //    };
        //    await _context.Cities.AddAsync(cat);
        //    _context.SaveChanges();
        //    return Ok();
        //}
    }
}
