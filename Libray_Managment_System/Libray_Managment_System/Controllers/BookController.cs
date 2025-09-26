using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Services.Book;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddBook(BookDTO dto)
    {
        await _bookService.AddBookAsync(dto);
        return Ok(new { message = "Book Added." });
    }

    [HttpGet("get all")]
    public async Task<IActionResult> GetBooks(FilterDTO filter)
    {
        var books = await _bookService.GetBooksAsync(filter);
        return Ok(books);
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateBook(BookDTO dto)
    {
        try
        {
            await _bookService.UpdateBookAsync(dto);
            return Ok(new { message = "Book Updated." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [HttpDelete("delete/{bookId}")]
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        try
        {
            await _bookService.DeleteBookAsync(bookId);
            return Ok(new { message = "Book Deleted." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetBookById(int bookId)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            return Ok(book);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}