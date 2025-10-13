using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Auth
{
    public interface IAuthService
    {
        Task<Result> RegisterUserAsync(RegisterDTO dto);
        Task<Result<string>>LoginUserAsync(LoginDTO dto);
    }
}
