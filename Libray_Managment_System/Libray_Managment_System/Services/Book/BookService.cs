using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.Book;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
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

        await _bookRepository.AddAsync(entity);
    }

    // Kitoblarni filter bilan olish
    public async Task<IEnumerable<BookDTO>> GetBooksAsync(FilterDTO filter)
    {
        var books = await _bookRepository.GetFilteredAsync(filter);

        return books.Select(b => new BookDTO
        {
            Id = b.Id,
            Title = b.Title,
            ISBN = b.Isbn,
            AuthorId = b.Authorid,
            CategoryId = b.Categoryid
        });
    }
    // Kitobni yangilash
    public async Task UpdateBookAsync(BookDTO dto)
    {
        var entity = await _bookRepository.GetByIdAsync(dto.Id);
        if (entity == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }
        entity.Title = dto.Title;
        entity.Isbn = dto.ISBN;
        entity.Authorid = dto.AuthorId;
        entity.Categoryid = dto.CategoryId;

        await _bookRepository.UpdateAsync(entity);
    }
    // Kitobni o‘chirish
    public async Task DeleteBookAsync(int bookId)
    {
        var entity = await _bookRepository.GetByIdAsync(bookId);
        if (entity == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }
        await _bookRepository.DeleteAsync(entity);
    }
    // Kitobni ID bo‘yicha olish
    public async Task<BookDTO> GetBookByIdAsync(int bookId)
    {
        var entity = await _bookRepository.GetByIdAsync(bookId);
        if (entity == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }
        return new BookDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            ISBN = entity.Isbn,
            AuthorId = entity.Authorid,
            CategoryId = entity.Categoryid
        };
    }
}