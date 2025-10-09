using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Services.Reserv;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveBook([FromBody] ReservationDto dto)
        {
            try
            {
                var result = await _reservationService.ReserveBookAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("approve/{reservationId}")]
        public async Task<IActionResult> ApproveReservation(int reservationId)
        {
            try
            {
                await _reservationService.ApproveReservationAsync(reservationId);
                return Ok("Approved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("cancel/{reservationId}")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            try
            {
                await _reservationService.CancelReservationAsync(reservationId);
                return Ok("Cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId)
        {
            try
            {
                var reservations = await _reservationService.GetUserReservationsAsync(userId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
