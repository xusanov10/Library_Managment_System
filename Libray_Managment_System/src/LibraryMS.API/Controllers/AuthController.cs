using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            return await _authService.RegisterUserAsync(dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            return await _authService.LoginUserAsync(dto);
        }
    }
}
