using AutoMapper;
using Libray_Managment_System.DtoModels.AuthorModels;
using Libray_Managment_System.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.AuthorService;

public class AuthorService : IAuthorService
{
    private readonly LibraryManagmentSystemContext _context;
    private readonly IMapper _mapper;

    public AuthorService(LibraryManagmentSystemContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorResponseDto>> GetAllAsync()
    {
        var authors = await _context.Authors.ToListAsync();
        return _mapper.Map<IEnumerable<AuthorResponseDto>>(authors);
    }

    public async Task<AuthorResponseDto?> GetByIdAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        return author == null ? null : _mapper.Map<AuthorResponseDto>(author);
    }

    public async Task<AuthorResponseDto> CreateAsync(AuthorCreateDto dto)
    {
        var author = _mapper.Map<Author>(dto);
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return _mapper.Map<AuthorResponseDto>(author);
    }

    public async Task<AuthorResponseDto?> UpdateAsync(int id, AuthorUpdateDto dto)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return null;

        _mapper.Map(dto, author);
        await _context.SaveChangesAsync();

        return _mapper.Map<AuthorResponseDto>(author);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return false;

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return true;
    }
}