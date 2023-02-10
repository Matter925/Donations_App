using Donations_App.Dtos.UserDto;
using Donations_App.Models;
using Donations_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfileSettingController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileSettingController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetProfileToUpdate/{email}")]
        public async Task<IActionResult> GetProfileData(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return BadRequest("Email should not be null");
            }
            var result = await _userService.GetProfileData(email);
            if (result == null)
            {
                return BadRequest("Email is incorrect or not found !!!");
            }
            return Ok(result);

        }

        [HttpPost("UpdateProfile/{email}")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto upProfile, string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateProfile(email, upProfile);
            if (result != null)
            {
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
                return Ok(result);
            }
            return NotFound("Email is incorrect or not found !!");

        }

        [HttpPost("ChangePassword/{email}")]
        public async Task<IActionResult> ChangePassword(string email, ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ChangePassword(email, model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost("ForgotPassword/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound(email);
            }
            var result = await _userService.ForgotPasswordAsync(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(VerifyCodeDto codeDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.VerifyCodeAsync(codeDto);
                if (result.Success)
                {
                    return Ok(result);

                }
                return NotFound(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("CreateNewPassword/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewPassword(string email , CreatePasswordDto model)
        {
            if(string.IsNullOrEmpty(email))
            {
                return BadRequest("Email should not be null");
            }
            if(ModelState.IsValid)
            {
                var result = await _userService.CreateNewPassword(email, model);
                if(result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(ModelState);

        }

        
    }
}
