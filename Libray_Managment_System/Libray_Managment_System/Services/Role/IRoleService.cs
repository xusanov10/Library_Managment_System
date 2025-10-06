using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Role
{
    public interface IRoleService
    {
        Task<Result> CreateRoleAsync(RoleDTO dto);
        Task<Result<IEnumerable<RoleDTO>>> GetAllRolesAsync();
        Task<Result<RoleDTO?>> GetRoleByIdAsync(int id);
        Task<Result> DeleteRoleAsync(int id);
        Task<Result> AssignPermissionAsync(RolePermissionDTO dto);
        Task<Result> UpdatePermissionsAsync(int roleId, List<int> permissionIds);
    }
}
