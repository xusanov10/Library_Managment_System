using Libray_Managment_System.Models;

namespace Libray_Managment_System.Services.ReportService;

public interface IReportRepository
{
    Task AddAsync(Report entity);
    Task<IEnumerable<Models.Book>> GetBorrowedBooksAsync(DateTime? from, DateTime? to);
}