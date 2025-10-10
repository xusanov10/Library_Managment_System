using Libray_Managment_System.Services.Fine;
using Microsoft.AspNetCore.Mvc;

namespace Library_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineController : ControllerBase
    {
        private readonly IFineService _fineService;
        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }
        [HttpGet("calculate/{borrowId}")]
        public async Task<IActionResult> CalculateFine(int borrowId)
        {
            try
            {
                var fine = await _fineService.CalculateFineAsync(borrowId);
                return Ok(new { BorrowId = borrowId, FineAmount = fine });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("create/{borrowId}")]
        public async Task<IActionResult> CreateFine(int borrowId)
        {
            try
            {
                var fine = await _fineService.CreateFineAsync(borrowId);
                return Ok(fine);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserFines(int userId)
        {
            try
            {
                var fines = await _fineService.GetUserFinesAsync(userId);
                return Ok(fines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
