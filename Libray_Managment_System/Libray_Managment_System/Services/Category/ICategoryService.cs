using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Category;

public interface ICategoryService
{
    Task AddCategoryAsync(CategoryDTO dto);
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task UpdateCategoryAsync(CategoryDTO dto);
    Task DeleteCategoryAsync(int categoryId);
}