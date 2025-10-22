using LibraryMS.Application.Models;
using LibraryMS.Application.Models.Role;
using LibraryMS.Application.Models.Role.Permisson;
using Libray_Managment_System.Services;

namespace LibraryMS.Application.Services
{
    public interface IRoleService
    {
        Task<Result> CreateRoleAsync(RoleResponseDTO dto);
        Task<Result<PaginationResult<RoleResponseDTO>>> GetAllRolesAsync(PageOptions rolePageDTO);
        Task<Result<RoleResponseDTO?>> GetRoleByIdAsync(int id);
        Task<Result> DeleteRoleAsync(int id);
        Task<Result> AssignPermissionAsync(RolePermissionDTO dto);
        Task<Result> UpdatePermissionsAsync(int roleId, List<int> permissionIds);
    }
}
