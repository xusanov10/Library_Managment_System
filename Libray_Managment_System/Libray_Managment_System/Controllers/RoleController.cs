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
        public async Task<IActionResult> CreateRole(RoleDTO dto)
        {
            var result = await _roleService.CreateRoleAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound("Role not found");

            return Ok(role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return Ok(result);
        }

        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermission(RolePermissionDTO dto)
        {
            var result = await _roleService.AssignPermissionAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> UpdateRolePermissions(int id, List<int> permissionIds)
        {
            var result = await _roleService.UpdatePermissionsAsync(id, permissionIds);

            if (result.Contains("not found"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
