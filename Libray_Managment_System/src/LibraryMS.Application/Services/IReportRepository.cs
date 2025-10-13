namespace LibraryMS.Application.Services;

public interface IReportRepository
{
    Task<IEnumerable<Models.Book>> GetBorrowedBooksAsync(DateTime from, DateTime to);
}