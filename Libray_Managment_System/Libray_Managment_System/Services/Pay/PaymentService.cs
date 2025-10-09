using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Pay
{
    public class PaymentService : IPaymentService
    {
        private readonly LibraryManagmentSystemContext _context;
        public PaymentService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }
        public async Task MakePaymentAsync(PaymentDto dto)
        {
            var fine = await _context.Fines.FindAsync(dto.FineId);
            if (fine == null)
            {
                throw new Exception("No such fine found");
            }
            if (fine.Paid != null && fine.Paid.Value)
            {
                throw new Exception("This fine has already been paid");
            }
            if (dto.Amount < fine.Amount)
            {
                throw new Exception("Payment amount cannot be less than the fine amount");
            }
            var payment = new Payment
            {
                UserId = dto.UserId,
                FineId = dto.FineId,
                Amount = dto.Amount,
                Paymentdate = DateTime.UtcNow,
                Method = dto.Method
            };
            _context.Payments.Add(payment);
            fine.Paid = true;
            await _context.SaveChangesAsync();
        }
        public async Task<List<PaymentResponseDto>> GetUserPaymentsAsync(int userId)
        {
            var payments = await _context.Payments
            .Where(p => p.UserId == userId)
            .ToListAsync();
            return payments.Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FineId = p.FineId,
                Amount = p.Amount,
                PaymentDate = (DateTime)p.Paymentdate,
                Method = p.Method
            }).ToList();
        }
        public async Task<PaymentResponseDto> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                throw new Exception("No such payment found");
            }
            return new PaymentResponseDto
            {
                Id = payment.Id,
                UserId = payment.UserId,
                FineId = payment.FineId,
                Amount = payment.Amount,
                PaymentDate = (DateTime)payment.Paymentdate,
                Method = payment.Method
            };
        }
        public async Task<List<PaymentResponseDto>> GetUnpaidFinesPaymentsAsync(int userId)
        {
            var payments = await _context.Payments
            .Where(p => p.UserId == userId && !_context.Fines
            .Where(f => f.Id == p.FineId)
            .Select(f => f.Paid ?? false)
            .FirstOrDefault())
            .ToListAsync();
            return payments.Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FineId = p.FineId,
                Amount = p.Amount,
                PaymentDate = (DateTime)p.Paymentdate,
                Method = p.Method
            }).ToList();
        }
    }
}
