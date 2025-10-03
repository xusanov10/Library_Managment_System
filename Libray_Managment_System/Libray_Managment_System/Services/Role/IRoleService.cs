using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Role
{
    public interface IRoleService
    {
        Task<Result> CreateRoleAsync(RoleDTO dto);
        Task<ResultDTO<IEnumerable<RoleDTO>>> GetAllRolesAsync();
        Task<ResultDTO<RoleDTO?>> GetRoleByIdAsync(int id);
        Task<Result> DeleteRoleAsync(int id);
        Task<Result> AssignPermissionAsync(RolePermissionDTO dto);
        Task<Result> UpdatePermissionsAsync(int roleId, List<int> permissionIds);
    }
}
