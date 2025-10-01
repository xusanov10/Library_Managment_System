using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Role
{
    public interface IRoleService
    {
        Task<ResultDTO> CreateRoleAsync(RoleDTO dto);
        Task<ResultDTO<IEnumerable<RoleDTO>>> GetAllRolesAsync();
        Task<ResultDTO<RoleDTO?>> GetRoleByIdAsync(int id);
        Task<ResultDTO> DeleteRoleAsync(int id);
        Task<ResultDTO> AssignPermissionAsync(RolePermissionDTO dto);
        Task<ResultDTO> UpdatePermissionsAsync(int roleId, List<int> permissionIds);
    }
}
