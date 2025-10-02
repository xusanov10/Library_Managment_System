using AutoMapper;
using Libray_Managment_System.DtoModels.CategoryModels;
using System;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly LibraryManagmentSystemContext _context;
    private readonly IMapper _mapper;

    public CategoryService(LibraryManagmentSystemContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category == null ? null : _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = _mapper.Map<Models.Category>(dto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<CategoryResponseDto?> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        _mapper.Map(dto, category);
        await _context.SaveChangesAsync();
        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}