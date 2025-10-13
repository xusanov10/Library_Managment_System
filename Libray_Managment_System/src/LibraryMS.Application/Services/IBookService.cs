using Libray_Managment_System.DtoModels;

namespace LibraryMS.Application.Services;

public interface IBookService
{
    Task AddBookAsync(BookDTO dto);
    Task<IEnumerable<BookDTO>> GetBooksAsync(FilterDTO filter);
}