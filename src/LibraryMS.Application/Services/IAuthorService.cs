using LibraryMS.Application.Models.Author;

namespace LibraryMS.Application.Services;

public interface IAuthorService
{
    Task AddAuthorAsync(AuthorDTO dto);
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
}