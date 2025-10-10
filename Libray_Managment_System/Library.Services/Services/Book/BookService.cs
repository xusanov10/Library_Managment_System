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
}