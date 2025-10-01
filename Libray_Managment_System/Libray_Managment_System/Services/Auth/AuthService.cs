using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Libray_Managment_System.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly LibraryManagmentSystemContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(LibraryManagmentSystemContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ResultDTO> RegisterUserAsync(RegisterDTO dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return new ResultDTO<string>
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

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

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

            return new ResultDTO
            {
                Message = "User registered successfully!",
                StatusCode = 201
            };
        }

        public async Task<ResultDTO<string>> LoginUserAsync(LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash))
                return new ResultDTO<string>
                {
                    Message = "Invalid email or password!",
                    StatusCode = 401,
                    Data = "error"
                };
            var token = _tokenService.GenerateToken(user);
            return new ResultDTO<string>
            {
                Message = "Login successful!",
                StatusCode = 200,
                Data = await token
            };
        }
    }
}
