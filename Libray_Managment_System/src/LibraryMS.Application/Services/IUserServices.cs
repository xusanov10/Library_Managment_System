using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Services;

namespace LibraryMS.Application.Services
{
    public interface IUserService
    {
        Task<Result<UserDTO?>> GetUserByIdAsync(int id);
        Task<Result<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<Result> DeleteUserAsync(int id);
        Task<Result<bool>> AssignRoleAsync(UserRoleDTO dto);
        Task<Result> UpdateUserAsync(int id, UpdateUserDTO dto);
        Task<Result<IEnumerable<UserDTO>>> GetUsersByRoleAsync(string roleName);
        Task<Result> UpdateUserProfileAsync(int id, UserProfileDTO dto);
        Task<Result> CreateUserProfileAsync(CreateUserProfileDTO dto);
    }
}
