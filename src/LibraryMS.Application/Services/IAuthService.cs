using LibraryMS.Application.Models.Auth;
using Libray_Managment_System.Services;

namespace LibraryMS.Application.Services
{
    public interface IAuthService
    {
        Task<Result> RegisterUserAsync(RegisterDTO dto);
        Task<Result<string>>LoginUserAsync(LoginDTO dto);
    }
}
