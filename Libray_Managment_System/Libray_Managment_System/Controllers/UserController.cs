using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("api/users/profile")]
        public async Task<IActionResult> CreateUserProfile(CreateUserProfileDTO dto)
        {
            var result = await _userService.CreateUserProfileAsync(dto);
            if (result == "User profile created successfully!")
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found!");
            return Ok(user);
        }
        [HttpGet("api/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpDelete("api/users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result == "User deleted successfully!")
                return Ok(result);
            return NotFound(result);
        }
        [HttpPost("api/users/assign-role")]
        public async Task<IActionResult> AssignRole(UserRoleDTO dto)
        {
            var result = await _userService.AssignRoleAsync(dto);
            if (result)
                return Ok("Role assigned successfully!");
            return BadRequest("Failed to assign role. Check if user and role exist.");
        }
        [HttpPut("api/users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (result == "User updated successfully!")
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("api/users/role/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = await _userService.GetUsersByRoleAsync(roleName);
            return Ok(users);
        }
        [HttpPut("api/users/{id}/profile")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfileDTO dto)
        {
            var result = await _userService.UpdateUserProfileAsync(id, dto);
            if (result == "User profile updated successfully!")
                return Ok(result);
            return BadRequest(result);
        }
    }
}
