using Libray_Managment_System.DtoModels.ReportModels;
using Libray_Managment_System.Services.ReportService;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _service;

    public ReportController(IReportService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportResponseDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ReportResponseDto>> GetById(int id)
    {
        var report = await _service.GetByIdAsync(id);
        return report == null ? NotFound() : Ok(report);
    }

    [HttpPost]
    public async Task<ActionResult<ReportResponseDto>> Create(ReportCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ReportResponseDto>> Update(int id, ReportUpdateDto dto)
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