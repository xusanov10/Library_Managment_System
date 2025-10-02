using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DtoModels.AuthorModels;

namespace Libray_Managment_System.Services.AuthorService;

public interface IAuthorService
{
    Task<IEnumerable<AuthorResponseDto>> GetAllAsync();
    Task<AuthorResponseDto?> GetByIdAsync(int id);
    Task<AuthorResponseDto> CreateAsync(AuthorCreateDto dto);
    Task<AuthorResponseDto?> UpdateAsync(int id, AuthorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}