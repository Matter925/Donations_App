using Donations_App.Models;
using Donations_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

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

        //[HttpPost("CreateRole")]
        //public async Task<IActionResult> CreateRole(CreateRoleDto createRole)
        //{

        //    var result = await _roleManager.CreateAsync(new IdentityRole
        //    {

        //        Name = createRole.RoleName
        //    });

        //    if (result.Succeeded)
        //    {
        //        return Ok("New Role Created");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}
    }
}
