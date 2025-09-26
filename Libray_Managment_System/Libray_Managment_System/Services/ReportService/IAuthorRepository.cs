namespace Libray_Managment_System.Services.ReportService;

public interface IAuthorRepository
{
    Task<IEnumerable<Models.Book>> GetBorrowedBooksAsync(DateTime from, DateTime to);
}