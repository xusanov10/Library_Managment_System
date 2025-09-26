using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Enum;

namespace Libray_Managment_System.Services.ReportService;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    public async Task<ReportDTO> GenerateReportAsync(ReportType type, DateTime? from = null, DateTime? to = null)
    {
        DateTime startDate;
        DateTime endDate = DateTime.UtcNow;

        switch (type)
        {
            case ReportType.Daily:
                startDate = DateTime.UtcNow.Date;
                break;
            case ReportType.Monthly:
                startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                break;
            case ReportType.Yearly:
                startDate = new DateTime(DateTime.UtcNow.Year, 1, 1);
                break;
            case ReportType.Custom:
                if (from == null || to == null)
                    throw new ArgumentException("Custom report requires 'from' and 'to' dates.");
                startDate = from.Value;
                endDate = to.Value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), "Unknown report type.");
        }

        var borrowedBooks = await _reportRepository.GetBorrowedBooksAsync(startDate, endDate);

        string data = $"Davr: {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}, " +
                      $"Qarzga berilgan kitoblar soni: {borrowedBooks.Count()}";

        return new ReportDTO
        {
            Type = type,
            GeneratedAt = DateTime.UtcNow,
            Data = data
        };
    }
}