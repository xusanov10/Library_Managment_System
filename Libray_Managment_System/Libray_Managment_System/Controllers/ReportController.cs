using Libray_Managment_System.Enum;
using Libray_Managment_System.Services.ReportService;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("generate")]
    public async Task<IActionResult> GenerateReport(
        [FromQuery] ReportType type,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        try
        {
            var report = await _reportService.GenerateReportAsync(type, from, to);
            return Ok(report);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Serverda xatolik yuz berdi." });
        }
    }
}