using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterDTO dto);
        Task<string> LoginUserAsync(LoginDTO dto);
    }
}
