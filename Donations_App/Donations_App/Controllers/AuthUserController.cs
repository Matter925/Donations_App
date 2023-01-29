using Donations_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Donations_App.Helpers;
using Microsoft.Extensions.Options;
using Donations_App.Services;
using Donations_App.Dtos.UserDto;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthUserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterAsync(register);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(login);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);

        }
        [HttpGet("GetProfileToUpdate/{email}")]
        public async Task<IActionResult> GetProfileData(string email)
        {
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

        [HttpGet("GetUserData/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _userService.GetUser(email);
            if (user.isNotNull)
            {
                return Ok(user);
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

        //[HttpPost("CreateRole")]
        //public async Task<IActionResult> CreateRole(CreateRoleDto createRole)
        //{

        //    var result = await _roleManager.CreateAsync(new IdentityRole
        //    {

        //        Name = createRole.RoleName
        //    }) ;

        //    if (result.Succeeded)
        //    {
        //        return Ok("New Role Created");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}
        [Authorize(Roles = "Admin")]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(AssignRoleDto assignRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.AssignRole(assignRole);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(assignRole);


        }
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _userService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _userService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }



    }
}
