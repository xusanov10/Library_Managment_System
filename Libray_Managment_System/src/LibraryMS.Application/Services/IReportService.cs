using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Enum;

namespace LibraryMS.Application.Services;

public interface IReportService
{
    Task<ReportDTO> GenerateReportAsync(ReportType type, DateTime? from = null, DateTime? to = null);
}