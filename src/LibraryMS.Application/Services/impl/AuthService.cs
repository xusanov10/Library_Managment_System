using Library_Management_System.Services;
using Library_Managment_System1;
using LibraryMS.Application.Models.Auth;
using LibraryMS.Application.Services;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Libray_Managment_System.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly LibraryManagmentSystemContext _context;
        public AuthService(LibraryManagmentSystemContext context)
        {
            _context = context;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash))
                return new Result<string>
                {
                    Message = "Invalid email or password!",
                    StatusCode = 401,
                    Data = "error"

                };
            var token = JwtHelper.GenerateJwtToken(user, _context);
            return new Result<string>
            {
                Message = "Login successful!",
                StatusCode = 200,
                Data = token
            };
        }
    }
}
