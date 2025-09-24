using Libray_Managment_System.Models;


public interface ITokenService
{
    string GenerateToken(User user);
}
