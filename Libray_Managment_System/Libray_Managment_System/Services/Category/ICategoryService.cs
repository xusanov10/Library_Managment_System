using Libray_Managment_System.DtoModels;
using Libray_Managment_System.DtoModels.CategoryModels;

namespace Libray_Managment_System.Services.Category;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(int id);
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryResponseDto?> UpdateAsync(int id, CategoryUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}