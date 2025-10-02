using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOs.LoginModels;
using Libray_Managment_System.DTOs.UserModels;

namespace Libray_Managment_System.Services.User
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterDTO dto);
        Task<string?> LoginUserAsync(LoginDTO dto);
        Task<bool> AssignRoleAsync(UserRoleDTO dto);
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    }
}
