using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Auth
{
    public interface IAuthService
    {
        Task<ResultDTO> RegisterUserAsync(RegisterDTO dto);
        Task<ResultDTO<string>>LoginUserAsync(LoginDTO dto);
    }
}
