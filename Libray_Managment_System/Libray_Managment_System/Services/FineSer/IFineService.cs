using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Fine
{
    public interface IFineService
    {
        Task<ResultDTO<int>> CalculateFineAsync(int borrowId);
        Task<ResultDTO<FineDto>> CreateFineAsync(int borrowId);
        Task<ResultDTO<List<FineDto>>> GetUserFinesAsync(int userId);
    }
}
