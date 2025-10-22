using LibraryMS.Application.Models.Fine;

namespace LibraryMS.Application.Services
{
    public interface IFineService
    {
        Task<int> CalculateFineAsync(int borrowId);
        Task<FineDTO> CreateFineAsync(int borrowId);
        Task<List<FineDTO>> GetUserFinesAsync(int userId);
    }
}
