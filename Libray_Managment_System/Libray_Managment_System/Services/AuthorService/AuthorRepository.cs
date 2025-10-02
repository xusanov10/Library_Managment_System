using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.AuthorService;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryManagmentSystemContext _context;
    public AuthorRepository(LibraryManagmentSystemContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Author entity)
    {
        _context.Authors.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task UpdateAsync(Author entity)
    {
        _context.Authors.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Author entity)
    {
        _context.Authors.Remove(entity);
        await _context.SaveChangesAsync();
    }
}