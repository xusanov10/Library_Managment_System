using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Services.AuthorService;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorDTO dto)
    {
        await _authorService.AddAuthorAsync(dto);
        return Ok(new { message = "Author Added." });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorService.GetAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{authorId}")]
    public async Task<IActionResult> GetAuthorById(int authorId)
    {
        try
        {
            var author = await _authorService.GetAuthorByIdAsync(authorId);
            return Ok(author);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorDTO dto)
    {
        try
        {
            await _authorService.UpdateAuthorAsync(dto);
            return Ok(new { message = "Author Updated." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAuthor([FromBody] AuthorDTO dto)
    {
        try
        {
            await _authorService.DeleteAuthorAsync(dto);
            return Ok(new { message = "Author Deleted." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}