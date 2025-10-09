using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Services.Borrow;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowDto dto)
        {
            try
            {
                var result = await _borrowService.BorrowBookAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("return/{borrowId}")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            try
            {
                await _borrowService.ReturnBookAsync(borrowId);
                return Ok("Book returned");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBorrows(int userId)
        {
            try
            {
                var borrows = await _borrowService.GetUserBorrowsAsync(userId);
                return Ok(borrows);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueBorrows()
        {
            try
            {
                var overdue = await _borrowService.GetOverdueBorrowsAsync();
                return Ok(overdue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
