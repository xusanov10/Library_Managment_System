using Library_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Fine
{
    public interface IFineService
    {
        Task<Result<int>> CalculateFineAsync(int borrowId);
        Task<Result<FineDTO>> CreateFineAsync(int borrowId);
        Task<Result<List<FineDTO>>> GetUserFinesAsync(int userId);
    }
}
