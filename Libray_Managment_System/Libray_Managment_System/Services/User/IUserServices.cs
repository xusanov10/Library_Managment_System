using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Users
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<string> DeleteUserAsync(int id);
        Task<bool> AssignRoleAsync(UserRoleDTO dto);
        Task<string> UpdateUserAsync(int id, UpdateUserDTO dto);
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName);
        Task<string> UpdateUserProfileAsync(int id, UserProfileDTO dto);
        Task<string> CreateUserProfileAsync(CreateUserProfileDTO dto);
    }
}
