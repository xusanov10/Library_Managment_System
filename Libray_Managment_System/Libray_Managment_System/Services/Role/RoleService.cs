using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Role
{
    public class RoleService : IRoleService
    {
        public Task<string> AssignPermissionAsync(RolePermissionDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateRoleAsync(RoleDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteRoleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RoleDTO?> GetRoleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
