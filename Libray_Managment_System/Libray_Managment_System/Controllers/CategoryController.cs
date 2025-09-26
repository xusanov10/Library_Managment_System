using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddCategory(CategoryDTO dto)
    {
        await _categoryService.AddCategoryAsync(dto);
        return Ok(new { message = "Category Added." });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCategory(CategoryDTO dto)
    {
        try
        {
            await _categoryService.UpdateCategoryAsync(dto);
            return Ok(new { message = "Category Updated." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/{categoryId}")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(categoryId);
            return Ok(new { message = "Category Deleted." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}