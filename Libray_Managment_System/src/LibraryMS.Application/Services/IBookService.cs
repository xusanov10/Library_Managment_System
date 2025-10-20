using LibraryMS.Application.Models.book;
using LibraryMS.Application.Models.Filter;

namespace LibraryMS.Application.Services;

public interface IBookService
{
    Task AddBookAsync(BookDTO dto);
    Task<IEnumerable<BookDTO>> GetBooksAsync(FilterDTO filter);
}