﻿using Donations_App.Models;
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
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _userService.getRoles();
            return Ok(result);
        }
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

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateRole(createRole);

            if (result)
                return Ok("Successfully");

            return BadRequest("The Role already exist !!");
        }
    }
}
