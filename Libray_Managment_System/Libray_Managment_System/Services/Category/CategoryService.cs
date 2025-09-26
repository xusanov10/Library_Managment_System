using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService (ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    // Create
    public async Task AddCategoryAsync(CategoryDTO dto)
    {
        var entity = new Models.Category
        {
            Name = dto.Name
        };

        await _categoryRepository.AddAsync(entity);
    }
    // Read
    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name
        });
    }

    public async Task UpdateCategoryAsync(CategoryDTO dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id);
        if (category == null)
            throw new KeyNotFoundException("Not Found");

        category.Name = dto.Name;
        await _categoryRepository.UpdateAsync(category);
    }

    public Task DeleteCategoryAsync(int categoryId)
    {
        var category = _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            throw new KeyNotFoundException("Not Found");

        return _categoryRepository.DeleteAsync(category.Result);
    }
}