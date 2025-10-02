using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Models;

namespace Libray_Managment_System.Services.AuthorService;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    public Task AddAuthorAsync(AuthorDTO dto)
    {
        var author = new Author
        {
            Fullname = dto.FullName,
            Birthyear = dto.BirthYear,
            Country = dto.Country,
            Deathyear = dto.DeathYear
        };
        return _authorRepository.AddAsync(author);
    }

    public async Task<IEnumerable<AuthorDTO>> GetAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors.Select(a => new AuthorDTO
        {
            Id = a.Id,
            FullName = a.Fullname
        });
    }
    public async Task DeleteAuthorAsync(AuthorDTO dto)
    {
        var author = await _authorRepository.GetByIdAsync(dto.Id);
        if (author == null)
        {
            throw new KeyNotFoundException("Author not found.");
        }
        await _authorRepository.DeleteAsync(author);
    }
    public async Task UpdateAuthorAsync(AuthorDTO dto)
    {
        var author = await _authorRepository.GetByIdAsync(dto.Id);
        if (author == null)
        {
            throw new KeyNotFoundException("Author not found.");
        }
        author.Fullname = dto.FullName;
        await _authorRepository.UpdateAsync(author);
    }
    public async Task<AuthorDTO> GetAuthorByIdAsync(int authorId)
    {
        var author = await _authorRepository.GetByIdAsync(authorId);
        if (author == null)
        {
            throw new KeyNotFoundException("Author not found.");
        }
        return new AuthorDTO
        {
            Id = author.Id,
            FullName = author.Fullname
        };
    }
}