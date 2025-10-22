using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;


public interface ITokenService
{
    Task<string> GenerateToken(User user);
}
