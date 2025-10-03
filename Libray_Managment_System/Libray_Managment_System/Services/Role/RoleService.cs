using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
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

        public async Task<Result> CreateRoleAsync(RoleDTO dto)
        {
            var exists = await _context.Roles.AnyAsync(r => r.Name == dto.Name);
            if (exists)
                return new Result
                {
                    Message = "Role already exists!",
                    StatusCode = 400,
                };

            var role = new Models.Role
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return new Result
            {
                Message = "Role created successfully!",
                StatusCode = 201,
            };
        }

        public async Task<ResultDTO<IEnumerable<RoleDTO>>> GetAllRolesAsync()
        {
            var role = await _context.Roles
                .Select(r => new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                })
                .ToListAsync();

            return new ResultDTO<IEnumerable<RoleDTO>>
            {
                Data = role,
                Message = "Roles retrieved successfully!",
                StatusCode = 200,
            };
        }

        public async Task<ResultDTO<RoleDTO>> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) 
               return new ResultDTO<RoleDTO>
               {
                   Message = "Role not found!",
                   StatusCode = 404,
               };

            return new ResultDTO<RoleDTO>
            {
                Data = new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                },
                Message = "Role retrieved successfully!",
                StatusCode = 200,
            };
        }

        public async Task<Result> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return new Result
                {
                    Message = "Role not found!",
                    StatusCode = 404,
                };

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return new Result
            {
                Message = "Role deleted successfully!",
                StatusCode = 200,
            };
        }

        public async Task<Result> AssignPermissionAsync(RolePermissionDTO dto)
        {
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.Roleid);
            var permExists = await _context.Permissions.AnyAsync(p => p.Id == dto.Permissionid);

            if (!roleExists || !permExists)
                return new Result
                {
                    Message = "Role or Permission not found!",
                    StatusCode = 404,
                };

            var alreadyAssigned = await _context.Rolepermissions
                .AnyAsync(rp => rp.Roleid == dto.Roleid && rp.Permissionid == dto.Permissionid);

            if (alreadyAssigned)
                return new Result
                {
                    Message = "Permission already assigned to role!",
                    StatusCode = 400,
                };

            var rolePermission = new Rolepermission
            {
                Roleid = dto.Roleid,
                Permissionid = dto.Permissionid
            };

            await _context.Rolepermissions.AddAsync(rolePermission);
            await _context.SaveChangesAsync();

            return new Result
            {
                Message = "Permission assigned to role successfully!",
                StatusCode = 201,
            };
        }
        public async Task<Result> UpdatePermissionsAsync(int roleId, List<int> permissionIds)
        {
            var role = await _context.Roles
                .Include(r => r.Rolepermissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                return new Result
                {
                    Message = "Role not found!",
                    StatusCode = 404,
                };

            _context.Rolepermissions.RemoveRange(role.Rolepermissions);

            foreach (var pid in permissionIds)
            {
                role.Rolepermissions.Add(new Rolepermission
                {
                    Roleid = role.Id,
                    Permissionid = pid
                });
            }

            await _context.SaveChangesAsync();
            return new Result
            {
                Message = "Role permissions updated successfully!",
                StatusCode = 200,
            };
        }
    }
}
