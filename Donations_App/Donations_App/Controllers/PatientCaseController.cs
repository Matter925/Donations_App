using Donations_App.Dtos.PatientCaseDtos;
using Donations_App.Repositories.PatientCaseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientCaseController : ControllerBase
    {
        private readonly IPatientCaseRepository _patientCaseRepository;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        public PatientCaseController(IPatientCaseRepository patientCaseRepository)
        {
            _patientCaseRepository = patientCaseRepository;
        }
        [HttpGet("GetAllPatientsCases")]
        public async Task<IActionResult> GetAll()
        {
            var Cases = await _patientCaseRepository.GetAllPatientsCases();
            return Ok(Cases);
        }

        [HttpGet("GetPatientCaseByID/{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var result = await _patientCaseRepository.GetPatientCaseByID(id);
            if(result == null)
            {
                return NotFound($"No patient case was found with ID {id} ");
            }
            return Ok(result);
        }

        [HttpGet("GetByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            var Cases = await _patientCaseRepository.GetByCategoryId(categoryId);
            return Ok(Cases);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreatePatientCase")]
        public async Task<IActionResult> AddPatientCase([FromForm]PatientCaseDto dto)
        {
            if(ModelState.IsValid)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");
                var result = await _patientCaseRepository.CreatePatientCase(dto);
                if(result == null)
                {
                    return BadRequest("The Patient Case is exist !!");
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePatientCase/{id}")]
        public async Task<IActionResult> DeletePatientCase(int id)
        {
            var result = await _patientCaseRepository.DeletePatientCase(id);
            if(result != null)
            {
                return Ok(result);  
            }
            return NotFound($"No patient case was found with ID {id} ");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdatePatientCase/{id}")]
        public async Task<IActionResult> UpdatePatientCase(int id, [FromForm]PatientCaseDto dto)
        {
            if(ModelState.IsValid)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg images are allowed!");
                }  
                var result = await _patientCaseRepository.UpdatePatientCase(id, dto);
                if(result == null)
                {
                    return NotFound($"No patient case was found with ID {id} ");
                }
                return Ok(result); 
            }
            return BadRequest(ModelState);

        }


    }
}
