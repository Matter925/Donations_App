using Donations_App.Dtos.RequestDtos;
using Donations_App.Repositories.RequestServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private new List<string> _allowedExtenstions = new List<string> { ".jpeg", ".webp" };
        public RequestController(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _requestRepository.GetAllRequests();
            return Ok(requests);

        }
       
        [HttpGet("GetUserRequests/{UserId}")]
        public async Task<IActionResult> GetByUserID(string UserId)
        {
            var requests = await _requestRepository.GetByUserID(UserId);

            return Ok(requests);
        }
       
        [HttpPost("ApplyRequest")]
        public async Task<IActionResult> ApplyRequest([FromForm] RequestDto dto)
        {
            if (ModelState.IsValid)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.ID_Photo.FileName).ToLower()) && !_allowedExtenstions.Contains(Path.GetExtension(dto.Medical_Report.FileName).ToLower()))
                    return BadRequest("Only .webp and .jpeg images are allowed!");
                var result = await _requestRepository.CreateRequest(dto);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("AccepteRequest/{RequestID}")]
        public async Task<IActionResult> AccepteRequest(int RequestID)
        {
            var result = await _requestRepository.AccepteRequest(RequestID);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("RejecteRequest/{RequestID}")]
        public async Task<IActionResult> RejecteRequest(int RequestID)
        {
            var result = await _requestRepository.RejecteRequest(RequestID);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("GetRequestByID/{RequestId}")]
        public async Task<IActionResult> GetRequestByID(int RequestId)
        {
            var result = await _requestRepository.GetRequestByID(RequestId);
            if (result == null)
            {
                return NotFound($"No request was found with ID: {RequestId}");
            }

            return Ok(result);
        }

        [HttpDelete("DeleteRequest/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _requestRepository.DeleteRequest(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
