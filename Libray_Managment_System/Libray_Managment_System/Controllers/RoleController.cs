using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Services.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromQuery]RoleDTO dto)
        {
           return await _roleService.CreateRoleAsync(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
           return await _roleService.GetAllRolesAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
           return await _roleService.GetRoleByIdAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
           return await _roleService.DeleteRoleAsync(id);
        }

        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermission([FromQuery]RolePermissionDTO dto)
        {
            return await _roleService.AssignPermissionAsync(dto);
        }

        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> UpdateRolePermissions(int id, List<int> permissionIds)
        {
           return await _roleService.UpdatePermissionsAsync(id, permissionIds);
        }
    }
}
