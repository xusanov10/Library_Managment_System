using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DtoModels.ReportModels;
using Libray_Managment_System.Enum;

namespace Libray_Managment_System.Services.ReportService;

public interface IReportService
{
    Task<IEnumerable<ReportResponseDto>> GetAllAsync();
    Task<ReportResponseDto?> GetByIdAsync(int id);
    Task<ReportResponseDto> CreateAsync(ReportCreateDto dto);
    Task<ReportResponseDto?> UpdateAsync(int id, ReportUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}