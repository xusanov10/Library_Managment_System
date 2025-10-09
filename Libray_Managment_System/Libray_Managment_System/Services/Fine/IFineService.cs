namespace Libray_Managment_System.Services.Fine
{
    public interface IFineService
    {
        Task<int> CalculateFineAsync(int borrowId);
        Task<FineDto> CreateFineAsync(int borrowId);
        Task<List<FineDto>> GetUserFinesAsync(int userId);
    }
}
