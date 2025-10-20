using LibraryMS.Application.Models;
using LibraryMS.Application.Models.Role;
using LibraryMS.Application.Models.User;
using LibraryMS.Application.Models.User.userProfil;
using Libray_Managment_System.Services;

namespace LibraryMS.Application.Services
{
    public interface IUserService
    {
        Task<Result<UserResponseDTO?>> GetUserByIdAsync(int id);
        Task<Result<PaginationResult<UserListResponseDTO>>> GetAllUsersAsync(PageOptions userPageDTO);
        Task<Result> DeleteUserAsync(int id);
        Task<Result<bool>> AssignRoleAsync(UserRoleDTO dto);
        Task<Result> UpdateUserAsync(int id, UpdateUserDTO dto);
        Task<Result<IEnumerable<PaginationResult<UserResponseDTO>>>> GetUsersByRoleAsync(PageOptions pageOptions, string roleName);
        Task<Result> UpdateUserProfileAsync(int id, UserProfileDTO dto);
        Task<Result> CreateUserProfileAsync(CreateUserProfileDTO dto);
    }
}
