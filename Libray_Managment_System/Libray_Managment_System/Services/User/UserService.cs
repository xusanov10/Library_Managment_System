using Library_Management_System.Services;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.Users;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly LibraryManagmentSystemContext _context;
    public UserService(LibraryManagmentSystemContext context)
    {
        _context = context;
    }

    public async Task<string> CreateUserProfileAsync(CreateUserProfileDTO dto)
    {
        var user = await _context.Users.FindAsync(dto.UserId);
        if (user == null)
            return "User not found!";

        var existingProfile = await _context.Userprofiles.FindAsync(dto.UserId);
        if (existingProfile != null)
            return "User already has a profile!";

        var profile = new Userprofile
        {
            Id = dto.UserId,
            Phonenumber = dto.PhoneNumber,
            Address = dto.Address,
            Birthdate = dto.BirthDate,
            Gender = dto.Gender
        };

        await _context.Userprofiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return "User profile created successfully!";
    }

    // Barcha foydalanuvchilarni olish
    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Fullname = u.Fullname,
                Email = u.Email,
                Status = u.Status
            })
            .ToListAsync();
    }

    // Foydalanuvchini ID orqali olish
    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDTO
        {
            Id = user.Id,
            Fullname = user.Fullname,
        };
    }

    // Role biriktirish
    public async Task<bool> AssignRoleAsync(UserRoleDTO dto)
    {
        var user = await _context.Users.FindAsync(dto.UserId);
        var role = await _context.Roles.FindAsync(dto.RoleId);

        if (user == null || role == null)
            return false;

        var userRole = new Userrole
        {
            Userid = dto.UserId,
            Roleid = dto.RoleId
        };

        await _context.Userroles.AddAsync(userRole);
        await _context.SaveChangesAsync();

        return true;
    }

    public Task<string> UpdateUserProfileAsync(int id, UserProfileDTO dto)
    {
        var user = _context.Userprofiles.AnyAsync(a => a.Id == id);
        if(user == null) return Task.FromResult("User not found");
        var updateUser = new Userprofile
        {
            Phonenumber = dto.Phonenumber,
            Address = dto.Address,
            Birthdate = dto.Birthdate
        };
        _context.Userprofiles.Update(updateUser);
        _context.SaveChanges();
        return Task.FromResult("User profile updated successfully");
    }

    public Task<string> DeleteUserAsync(int id)
    {
        var user = _context.Users.FindAsync(id);
        if (user == null) return Task.FromResult("User not found");
        _context.Users.Remove(user.Result);
        _context.SaveChanges();
        return Task.FromResult("User deleted successfully");
    }

    public async Task<string> UpdateUserAsync(int id, UpdateUserDTO dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return "User not found!";

        if (!string.IsNullOrWhiteSpace(dto.FullName))
            user.Fullname = dto.FullName;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email;

        if (dto.Status.HasValue)
            user.Status = dto.Status;

        await _context.SaveChangesAsync();
        return "User updated successfully!";
    }
     public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName)
    {
        var users = await _context.Users
            .Where(u => u.Userroles.Any(ur => ur.Role.Name == roleName))
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Fullname = u.Fullname,
                Email = u.Email,
                Status = u.Status
            })
            .ToListAsync();

        return users;
    }

}
