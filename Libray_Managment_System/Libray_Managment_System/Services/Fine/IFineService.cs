using Library_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Fine
{
    public interface IFineService
    {
        Task<int> CalculateFineAsync(int borrowId);
        Task<FineDTO> CreateFineAsync(int borrowId);
        Task<List<FineDTO>> GetUserFinesAsync(int userId);
    }
}
