using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.AuthorService;

public interface IAuthorService
{
    Task AddAuthorAsync(AuthorDTO dto);
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
    Task DeleteAuthorAsync(AuthorDTO dto);
    Task UpdateAuthorAsync(AuthorDTO dto);
    Task<AuthorDTO> GetAuthorByIdAsync(int authorId);
}