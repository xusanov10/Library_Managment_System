using Libray_Managment_System.Models;

namespace Libray_Managment_System.Services.AuthorService;

public interface IAuthorRepository
{
    Task AddAsync(Author entity);
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author> GetByIdAsync(int id);
    Task UpdateAsync(Author entity);
    Task DeleteAsync(Author entity);
}