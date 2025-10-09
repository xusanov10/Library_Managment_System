using System.Security.Claims;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Services.Pay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("pay")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentDto dto)
        {
            try
            {
                await _paymentService.MakePaymentAsync(dto);
                return Ok("Payment done");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPayments(int userId)
        {
            try
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(claim) || !int.TryParse(claim, out var tokenId))
                {
                    return Unauthorized("Invalid user ID");
                }
                if (tokenId != userId && !User.IsInRole("Admin"))
                {
                    return Unauthorized("Access denied");
                }
                var payments = await _paymentService.GetUserPaymentsAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
                if (payment == null) return NotFound("Payment not found");
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("unpaid/{userId}")]
        public async Task<IActionResult> GetUnpaidFinesPayments(int userId)
        {
            try
            {
                var payments = await _paymentService.GetUnpaidFinesPaymentsAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
