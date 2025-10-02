using AutoMapper;
using Libray_Managment_System.DTOs.BookModels;
using System;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Book;

public class BookService : IBookService
{
    private readonly LibraryManagmentSystemContext _context;
    private readonly IMapper _mapper;

    public BookService(LibraryManagmentSystemContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        var books = await _context.Books.ToListAsync();
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public async Task<BookResponseDto?> GetByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        return book == null ? null : _mapper.Map<BookResponseDto>(book);
    }

    public async Task<BookResponseDto> CreateAsync(BookCreateDto dto)
    {
        var book = _mapper.Map<Models.Book>(dto);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<BookResponseDto?> UpdateAsync(int id, BookUpdateDto dto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;

        _mapper.Map(dto, book);
        await _context.SaveChangesAsync();

        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}