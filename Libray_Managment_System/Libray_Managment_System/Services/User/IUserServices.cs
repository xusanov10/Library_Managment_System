using Libray_Managment_System.DtoModels;

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
