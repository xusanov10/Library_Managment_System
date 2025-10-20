using Library_Managment_System1;
using LibraryMS.Application.Models.book;
using LibraryMS.Application.Models.Filter;
using LibraryMS.Application.Services;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly LibraryManagmentSystemContext _context;
    public BookService(LibraryManagmentSystemContext context)
    {
        _context = context;
    }

    // Kitob qo‘shish
    public async Task AddBookAsync(BookDTO dto)
    {
        var entity = new Book
        {
            Title = dto.Title,
            Isbn = dto.ISBN,
            Authorid = dto.AuthorId,
            Categoryid = dto.CategoryId
        };
        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // Kitoblarni filter bilan olish
    public async Task<IEnumerable<BookDTO>> GetBooksAsync(FilterDTO filter)
    { 
        var query = _context.Books.AsQueryable();
        if (!string.IsNullOrEmpty(filter.Title))
        {
            query = query.Where(b => b.Title.Contains(filter.Title));
        }
        if (filter.AuthorId.HasValue)
        {
            query = query.Where(b => b.Authorid == filter.AuthorId.Value);
        }
        if (filter.CategoryId.HasValue)
        {
            query = query.Where(b => b.Categoryid == filter.CategoryId.Value);
        }
        var books = await query.Select(b => new BookDTO
        {
            Id = b.Id,
            Title = b.Title,
            ISBN = b.Isbn,
            AuthorId = b.Authorid,
            CategoryId = b.Categoryid
        }).ToListAsync();
        return books;
    }
}