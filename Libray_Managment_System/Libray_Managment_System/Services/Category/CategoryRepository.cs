using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Category;

public class CategoryRepository : ICategoryRepository
{
    private readonly LibraryManagmentSystemContext _context;
    public CategoryRepository(LibraryManagmentSystemContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Models.Category entity)
    {
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Models.Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Models.Category> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task UpdateAsync(Models.Category entity)
    {
        _context.Categories.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Models.Category entity)
    {
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();
    }
}