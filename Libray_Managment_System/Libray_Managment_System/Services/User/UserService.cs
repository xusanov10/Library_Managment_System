using Library_Management_System.Services;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services;
using Libray_Managment_System.Services.Users;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly LibraryManagmentSystemContext _context;
    private readonly IFileStorageService _fileStorageService;

    public UserService(LibraryManagmentSystemContext context, IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
        _context = context;
    }

   /* public async Task<Result> CreateUserProfileAsync(CreateUserProfileDTO dto)
    {
        // Foydalanuvchi mavjudligini tekshirish
        var userExists = await _context.Users.AnyAsync(x => x.Id == dto.UserId);
        if (!userExists)
            return new Result
            {
                Message = "User not found!",
                StatusCode = 404,
            };

        // Profil mavjudligini tekshirish
        var existingProfile = await _context.Userprofiles.FindAsync(dto.UserId);
        if (existingProfile != null)
            return new Result
            {
                Message = "User profile already exists!",
                StatusCode = 400,
            };

        string? profilePictureUrl = null;

        // 📸 Agar rasm yuborilgan bo‘lsa — MinIO ga yuklash
        if (dto.ProfilePicture != null)
        {
            var bucketName = "user-profile-pictures";
            var objectName = $"{Guid.NewGuid()}_{dto.ProfilePicture.FileName}";

            using var stream = dto.ProfilePicture.OpenReadStream();

            var uploadResult = await _fileStorageService.UploadFileAsync(
                bucketName,
                objectName,
                stream,
                dto.ProfilePicture.ContentType
            );

            // ❌ IsSuccess o‘rniga StatusCode tekshiramiz
            if (uploadResult.StatusCode != 200 && uploadResult.StatusCode != 201)
            {
                return new Result
                {
                    Message = "Failed to upload profile picture!",
                    StatusCode = 500
                };
            }

            // Fayl URL ni olamiz
            profilePictureUrl = uploadResult.Value;
        }

        // ✅ Profil yaratish
        var profile = new Userprofile
        {
            Id = dto.UserId,
            Phonenumber = dto.PhoneNumber,
            Address = dto.Address,
            Birthdate = dto.BirthDate,
            Gender = dto.Gender,
            ProfilePictureUrl = profilePictureUrl // yangi property
        };

        await _context.Userprofiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User profile created successfully!",
            StatusCode = 201,
        };
    }
*/
    // Barcha foydalanuvchilarni olish
    public async Task<Result<IEnumerable<UserDTO>>> GetAllUsersAsync()
    {
        var result = await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Fullname = u.Fullname,
                Email = u.Email,
                Status = u.Status
            })
            .ToListAsync();
        return new Result<IEnumerable<UserDTO>>
        {
            Data = result,
            Message = "Users retrieved successfully!",
            StatusCode = 200,
        };
    }

    // Foydalanuvchini ID orqali olish
    public async Task<Result<UserDTO?>> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new Result<UserDTO?>
        {
            Data = new UserDTO
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                Status = user.Status
            },
            Message = "User retrieved successfully!",
            StatusCode = 200,
        };
    }

    // Role biriktirish
    public async Task<Result<bool>> AssignRoleAsync(UserRoleDTO dto)
    {
        var user = await _context.Users.FindAsync(dto.UserId);
        var role = await _context.Roles.FindAsync(dto.RoleId);

        if (user == null || role == null)
            return new Result<bool>
            {
                Data = false,
                Message = "User or Role not found!",
                StatusCode = 404,
            };

        var userRole = new Userrole
        {
            Userid = dto.UserId,
            Roleid = dto.RoleId
        };

        await _context.Userroles.AddAsync(userRole);
        await _context.SaveChangesAsync();

        return new Result<bool>
        {
            Data = true,
            Message = "Role assigned successfully!",
            StatusCode = 200,
        };
    }

    public async Task<Result> UpdateUserProfileAsync(int id, UserProfileDTO dto)
    {
        var user = await _context.Userprofiles.AnyAsync(a => a.Id == id);
        if (user) return new Result
        {
            Message = "User profile not found",
            StatusCode = 404
        };
        var updateUser = new Userprofile
        {
            Phonenumber = dto.Phonenumber,
            Address = dto.Address,
            Birthdate = dto.Birthdate
        };
        _context.Userprofiles.Update(updateUser);
        _context.SaveChanges();
        return new Result
        {
            Message = "User profile updated successfully",
            StatusCode = 200
        };
    }

    public async Task<Result> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return new Result
        {
            Message = "User not found",
            StatusCode = 404
        };
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return new Result
        {
            Message = "User deleted successfully!",
            StatusCode = 200
        };
    }

    public async Task<Result> UpdateUserAsync(int id, UpdateUserDTO dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return new Result
            {
                Message = "User not found!",
                StatusCode = 404,
            };

        if (!string.IsNullOrWhiteSpace(dto.FullName))
            user.Fullname = dto.FullName;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email;

        if (dto.Status.HasValue)
            user.Status = dto.Status;

        await _context.SaveChangesAsync();
        return new Result
        {
            Message = "User updated successfully!",
            StatusCode = 200,
        };
    }
    public async Task<Result<IEnumerable<UserDTO>>> GetUsersByRoleAsync(string roleName)
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

        return new Result<IEnumerable<UserDTO>>
        {
            Data = users,
            Message = "Users with the specified role retrieved successfully!",
            StatusCode = 200,
        };
    }
}
