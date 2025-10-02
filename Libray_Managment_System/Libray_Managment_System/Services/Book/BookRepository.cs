using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Book;

public class BookRepository : IBookRepository
{
    private readonly LibraryManagmentSystemContext _context;
    public BookRepository(LibraryManagmentSystemContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Models.Book entity)
    {
        _context.Books.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Models.Book>> GetFilteredAsync(FilterDTO filter)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Title))
            query = query.Where(b => b.Title.Contains(filter.Title));

        if (filter.AuthorId.HasValue)
            query = query.Where(b => b.Authorid == filter.AuthorId.Value);

        if (filter.CategoryId.HasValue)
            query = query.Where(b => b.Categoryid == filter.CategoryId.Value);

        return await query.ToListAsync();
    }

    public async Task<Models.Book> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task UpdateAsync(Models.Book entity)
    {
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Models.Book entity)
    {
        _context.Books.Remove(entity);
        await _context.SaveChangesAsync();
    }
}