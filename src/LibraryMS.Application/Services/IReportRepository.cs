using Libray_Managment_System.Models;

namespace LibraryMS.Application.Services;

public interface IReportRepository
{
    Task<IEnumerable<Book>> GetBorrowedBooksAsync(DateTime from, DateTime to);
}