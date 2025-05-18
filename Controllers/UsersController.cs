using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;
using bocchiwebbackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bocchiwebbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return this.ApiModelStateErrors<UserDto>(ModelState);
            }

            var result = await _userService.RegisterAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return this.ApiModelStateErrors<AuthResponseDto>(ModelState);
            }

            var result = await _userService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize] // Only authenticated users can access
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only admins can see all users
        public async Task<ActionResult<ApiResponse<PagedList<UserDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetAllAsync(page, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize] // Users should only update their own profile - this would be enforced in the service
        public async Task<ActionResult<ApiResponse<UserDto>>> Update(int id, UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return this.ApiModelStateErrors<UserDto>(ModelState);
            }

            var result = await _userService.UpdateAsync(id, updateUserDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete users
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetProfile()
        {
            // Get the current user's ID from the claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return this.ApiUnauthorized<UserDto>("Invalid user token");
            }

            var result = await _userService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}