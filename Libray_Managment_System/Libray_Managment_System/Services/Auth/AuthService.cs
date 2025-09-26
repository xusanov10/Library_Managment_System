using Libray_Managment_System.DtoModels;
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
        private readonly IConfiguration _config;

        public AuthService(LibraryManagmentSystemContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

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

        public async Task<string?> LoginUserAsync(LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash))
                return null;

            // JWT token yaratish
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Fullname)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
