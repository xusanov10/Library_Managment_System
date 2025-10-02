using Libray_Managment_System.DtoModels.AuthorModels;
using Libray_Managment_System.Services.AuthorService;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _service;

    public AuthorController(IAuthorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorResponseDto>> GetById(int id)
    {
        var author = await _service.GetByIdAsync(id);
        return author == null ? NotFound() : Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorResponseDto>> Create(AuthorCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuthorResponseDto>> Update(int id, AuthorUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}