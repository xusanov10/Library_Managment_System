using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.ReportService;

public class ReportRepository : IReportRepository
{
    private readonly LibraryManagmentSystemContext _context;

    public ReportRepository(LibraryManagmentSystemContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Report entity)
    {
        _context.Reports.Add(entity);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Models.Book>> GetBorrowedBooksAsync(DateTime? from, DateTime? to)
    {
        throw new NotImplementedException();
    }
}