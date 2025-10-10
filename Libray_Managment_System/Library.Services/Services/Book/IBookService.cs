using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Book;

public interface IBookService
{
    Task AddBookAsync(BookDTO dto);
    Task<IEnumerable<BookDTO>> GetBooksAsync(FilterDTO filter);
}