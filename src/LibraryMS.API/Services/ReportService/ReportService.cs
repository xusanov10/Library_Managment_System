using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Enum;

namespace Libray_Managment_System.Services.ReportService;

public class ReportService : IReportService
{
    public Task<ReportDTO> GenerateReportAsync(ReportType type, DateTime? from = null, DateTime? to = null)
    {
        throw new NotImplementedException();
    }
}