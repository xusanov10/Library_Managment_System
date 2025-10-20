using Library_Management_System.Services;
using Library_Managment_System1;
using LibraryMS.Application.Models.Auth;
using LibraryMS.Application.Services;
using LibraryMS.DataAccess.Authentication;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Libray_Managment_System.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly LibraryManagmentSystemContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthService(LibraryManagmentSystemContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result> RegisterUserAsync(RegisterDTO dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return new Result<string>
                {
                    Message = "Email already in use!",
                    StatusCode = 400,
                };
            var user = new User
            {
                Fullname = dto.FullName,
                Email = dto.Email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Status = true,
                Createdat = DateTime.UtcNow
            };

            var studentRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
            if (studentRole != null)
            {
                var userRole = new Userrole
                {
                    Userid = user.Id,
                    Roleid = studentRole.Id
                };
                await _context.Userroles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }

            return new Result
            {
                Message = "User registered successfully!",
                StatusCode = 201
            };
        }

        public async Task<Result<string>> LoginUserAsync(LoginDTO dto)
        {
            // 1️⃣ Foydalanuvchini email orqali topamiz
            var user = await _context.Users
                .Include(u => u.Userroles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return new Result<string>
                {
                    StatusCode = 401,
                    Message = "User not found!",
                    Data = "error"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash);
            if (!isPasswordValid)
            {
                return new Result<string>
                {
                    StatusCode = 401,
                    Message = "Invalid password!",
                    Data = "error"
                };
            }

            var role = user.Userroles.FirstOrDefault()?.Role?.Name ?? "Student";

            var token = _jwtTokenService.GenerateToken(user.Id.ToString(), role);

            return new Result<string>
            {
                StatusCode = 200,
                Message = "Login successful!",
                Data = token
            };
        }
    }
}
