using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Users
{
    public interface IUserService
    {
        Task<ResultDTO<UserDTO?>> GetUserByIdAsync(int id);
        Task<ResultDTO<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<Result> DeleteUserAsync(int id);
        Task<ResultDTO<bool>> AssignRoleAsync(UserRoleDTO dto);
        Task<Result> UpdateUserAsync(int id, UpdateUserDTO dto);
        Task<ResultDTO<IEnumerable<UserDTO>>> GetUsersByRoleAsync(string roleName);
        Task<Result> UpdateUserProfileAsync(int id, UserProfileDTO dto);
        Task<Result> CreateUserProfileAsync(CreateUserProfileDTO dto);
    }
}
