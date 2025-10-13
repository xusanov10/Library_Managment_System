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
           return await _userService.CreateUserProfileAsync(dto);
        }
        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
          return await _userService.GetUserByIdAsync(id);
        }
        [HttpGet("api/users")]
        public async Task<IActionResult> GetAllUsers()
        {
          return await _userService.GetAllUsersAsync();
        }
        [HttpDelete("api/users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
           return await _userService.DeleteUserAsync(id);
        }
        [HttpPost("api/users/assign-role")]
        public async Task<IActionResult> AssignRole(UserRoleDTO dto)
        {
            return await _userService.AssignRoleAsync(dto);
        }
        [HttpPut("api/users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO dto)
        {
            return await _userService.UpdateUserAsync(id, dto);
        }
        [HttpGet("api/users/role/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
           return await _userService.GetUsersByRoleAsync(roleName);
        }
        [HttpPut("api/users/{id}/profile")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfileDTO dto)
        {
            return await _userService.UpdateUserProfileAsync(id, dto);
        }
    }
}
