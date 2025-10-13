using Libray_Managment_System.DtoModels;

namespace LibraryMS.Application.Services;

public interface IAuthorService
{
    Task AddAuthorAsync(AuthorDTO dto);
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
}