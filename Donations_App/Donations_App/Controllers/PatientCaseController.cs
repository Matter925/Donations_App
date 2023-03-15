using Donations_App.Dtos.PatientCaseDtos;
using Donations_App.Repositories.PatientCaseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    
    public class PatientCaseController : ControllerBase
    {
        private readonly IPatientCaseRepository _patientCaseRepository;
        private new List<string> _allowedExtenstions = new List<string> { ".jpeg" , ".webp" ,".svg" };
        public PatientCaseController(IPatientCaseRepository patientCaseRepository)
        {
            _patientCaseRepository = patientCaseRepository;
        }
        
        [HttpGet("GetAllPatientsCases/{page=0}/{limit=0}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int page =0 , int limit = 0)
        {
            var Cases = await _patientCaseRepository.GetAllPatientsCases(page , limit);
            return Ok(Cases);
        }

        [HttpGet("Completed_Cases/{page=0}/{limit=0}")]
        [AllowAnonymous]
        public async Task<IActionResult> CompletedPatientsCases(int page = 0, int limit = 0)
        {
            var Cases = await _patientCaseRepository.CompletedPatientsCases(page , limit);
            return Ok(Cases);
        }
        [HttpGet("GetPatientCaseByID/{id}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            var Cases = await _patientCaseRepository.GetByCategoryId(categoryId);
            return Ok(Cases);

        }

        [HttpPost("CreatePatientCase")]
        public async Task<IActionResult> AddPatientCase([FromForm]PatientCaseDto dto)
        {
            if(ModelState.IsValid)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    return BadRequest("Only .webp and .jpeg images are allowed!");
                var result = await _patientCaseRepository.CreatePatientCase(dto);
                if(result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }

        
        [HttpDelete("DeletePatientCase/{id}")]
        public async Task<IActionResult> DeletePatientCase(int id)
        {
            var result = await _patientCaseRepository.DeletePatientCase(id);
            if(result.Success)
            {
                return Ok(result);  
            }
            return NotFound(result);
        }

        //[Authorize]
        //[HttpPut("IncreamentAmountPaid")]
        //public async Task<IActionResult> IncreamentAmountPaid(IncreamentAmountDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _patientCaseRepository.IncreamentAmountPaid(dto);
        //        if (result.Success)
        //        {
        //            return Ok(result);
        //        }
        //        return NotFound(result);
        //    }
        //    return BadRequest(ModelState);
        //}


        [HttpPut("UpdatePatientCase/{id}")]
        public async Task<IActionResult> UpdatePatientCase(int id, [FromForm]PatientCaseDto dto)
        {
            if(ModelState.IsValid)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                {
                    return BadRequest("Only .webp and .jpeg images are allowed!");
                }  
                var result = await _patientCaseRepository.UpdatePatientCase(id, dto);
                if(result.Success)
                {
                    return Ok(result);
                }
                return NotFound(result); 
            }
            return BadRequest(ModelState);

        }


    }
}
