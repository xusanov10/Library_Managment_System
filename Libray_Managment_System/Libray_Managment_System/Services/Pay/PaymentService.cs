
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Pay
{
    public class PaymentService : IPaymentService
    {
        private readonly LibraryManagmentSystemContext _context;
        public PaymentService(LibraryManagmentSystemContext context) => _context = context;

        public async Task<ResultDTO<string>> MakePaymentAsync(PaymentDto dto)
        {
            var result = new ResultDTO<string>();

            try
            {
                var fine = await _context.Fines.FindAsync(dto.FineId);
                if (fine == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Fine not found";
                    return result;
                }

                if (fine.Paid == true)
                {
                    result.StatusCode = 400;
                    result.Message = "This fine has already been paid";
                    return result;
                }

                if (dto.Amount < fine.Amount)
                {
                    result.StatusCode = 400;
                    result.Message = "Payment amount cannot be less than the fine amount";
                    return result;
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

                result.StatusCode = 200;
                result.Message = "Payment successful";
                result.Data = "Payment recorded successfully";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Server error: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultDTO<List<PaymentResponseDto>>> GetUserPaymentsAsync(int userId)
        {
            var result = new ResultDTO<List<PaymentResponseDto>>();

            try
            {
                var payments = await _context.Payments
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                result.Data = payments.Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    FineId = p.FineId,
                    Amount = p.Amount,
                    PaymentDate = (DateTime)p.Paymentdate,
                    Method = p.Method
                }).ToList();

                result.StatusCode = 200;
                result.Message = "User payments fetched successfully";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error fetching payments: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultDTO<PaymentResponseDto>> GetPaymentByIdAsync(int paymentId)
        {
            var result = new ResultDTO<PaymentResponseDto>();

            try
            {
                var payment = await _context.Payments.FindAsync(paymentId);
                if (payment == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Payment not found";
                    return result;
                }

                result.Data = new PaymentResponseDto
                {
                    Id = payment.Id,
                    UserId = payment.UserId,
                    FineId = payment.FineId,
                    Amount = payment.Amount,
                    PaymentDate = (DateTime)payment.Paymentdate,
                    Method = payment.Method
                };

                result.StatusCode = 200;
                result.Message = "Payment fetched successfully";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error fetching payment: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultDTO<List<PaymentResponseDto>>> GetUnpaidFinesPaymentsAsync(int userId)
        {
            var result = new ResultDTO<List<PaymentResponseDto>>();

            try
            {
                var unpaidFines = await _context.Fines
                    .Where(f => f.UserId == userId && (f.Paid == null || f.Paid == false))
                    .Select(f => f.Id)
                    .ToListAsync();

                var payments = await _context.Payments
                    .Where(p => unpaidFines.Contains(p.FineId))
                    .ToListAsync();

                result.Data = payments.Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    FineId = p.FineId,
                    Amount = p.Amount,
                    PaymentDate = (DateTime)p.Paymentdate,
                    Method = p.Method
                }).ToList();

                result.StatusCode = 200;
                result.Message = "Unpaid fines payments fetched successfully";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error fetching unpaid fines: {ex.Message}";
            }

            return result;
        }
    }
}

