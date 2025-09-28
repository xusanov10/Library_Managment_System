using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Role
{
    public interface IRoleService
    {
        Task<string> CreateRoleAsync(RoleDTO dto);
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO?> GetRoleByIdAsync(int id);
        Task<string> DeleteRoleAsync(int id);
        Task<string> AssignPermissionAsync(RolePermissionDTO dto);
        Task<string> UpdatePermissionsAsync(int roleId, List<int> permissionIds);
    }
}
