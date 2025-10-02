using Libray_Managment_System.DTOs.BookModels;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAll()
    {
        return Ok(await _bookService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDto>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create(BookCreateDto dto)
    {
        var created = await _bookService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookResponseDto>> Update(int id, BookUpdateDto dto)
    {
        var updated = await _bookService.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}