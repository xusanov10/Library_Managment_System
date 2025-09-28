using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly LibraryManagmentSystemContext _context;

        public RoleService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }

        public async Task<string> CreateRoleAsync(RoleDTO dto)
        {
            var exists = await _context.Roles.AnyAsync(r => r.Name == dto.Name);
            if (exists)
                return "Role already exists!";

            var role = new Models.Role
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return "Role created successfully!";
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                })
                .ToListAsync();
        }

        public async Task<RoleDTO?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public async Task<string> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return "Role not found!";

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return "Role deleted successfully!";
        }

        public async Task<string> AssignPermissionAsync(RolePermissionDTO dto)
        {
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.Roleid);
            var permExists = await _context.Permissions.AnyAsync(p => p.Id == dto.Permissionid);

            if (!roleExists || !permExists)
                return "Role or Permission not found!";

            var alreadyAssigned = await _context.Rolepermissions
                .AnyAsync(rp => rp.Roleid == dto.Roleid && rp.Permissionid == dto.Permissionid);

            if (alreadyAssigned)
                return "Permission already assigned to this role!";

            var rolePermission = new Rolepermission
            {
                Roleid = dto.Roleid,
                Permissionid = dto.Permissionid
            };

            await _context.Rolepermissions.AddAsync(rolePermission);
            await _context.SaveChangesAsync();

            return "Permission assigned to role successfully!";
        }
    }
}
