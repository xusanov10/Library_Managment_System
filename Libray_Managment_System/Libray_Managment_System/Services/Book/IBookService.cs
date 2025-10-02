using Libray_Managment_System.DTOs.BookModels;

namespace Libray_Managment_System.Services.Book;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto?> GetByIdAsync(int id);
    Task<BookResponseDto> CreateAsync(BookCreateDto dto);
    Task<BookResponseDto?> UpdateAsync(int id, BookUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}