using Library_Management_System.Services;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOs.LoginModels;
using Libray_Managment_System.DTOs.UserModels;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.User;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly LibraryManagmentSystemContext _context;
    private readonly TokenService _tokenService;
    public UserService(LibraryManagmentSystemContext context)
    {
        _context = context;
    }

    // Foydalanuvchini ro‘yxatdan o‘tkazish
    public async Task<string> RegisterUserAsync(RegisterDTO dto)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists)
            return "User already exists!";

        var user = new User
            {
            Fullname = dto.FullName,
            Email = dto.Email,
            Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Status = true,
            Createdat = DateTime.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return "User registered successfully!";
    }

    // Login
    public async Task<string?> LoginUserAsync(LoginDTO dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash);
        if (!isPasswordValid) return null;

        return _tokenService.GenerateToken(user);
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
}
