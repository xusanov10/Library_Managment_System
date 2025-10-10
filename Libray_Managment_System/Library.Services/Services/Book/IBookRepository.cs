using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Book;

public interface IBookRepository
{
    Task AddAsync(Models.Book entity);
    Task<IEnumerable<Models.Book>> GetFilteredAsync(FilterDTO filter);
}