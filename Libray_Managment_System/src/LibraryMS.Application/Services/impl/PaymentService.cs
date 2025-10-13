using Library_Managment_System.DTOModels;
using Library_Managment_System.Services.Payment;
using Library_Managment_System;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Pay
{
    public class PaymentService : IPaymentService
    {
        private readonly LibraryManagmentSystemContext _context;
        public PaymentService(LibraryManagmentSystemContext context) => _context = context;

        public async Task<Result<string>> MakePaymentAsync(PaymentDTO dto)
        {
            var result = new Result<string>();

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
                    Userid = dto.UserId,
                    Fineid = dto.FineId,
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

        public async Task<Result<List<PaymentResponseDTO>>> GetUserPaymentsAsync(int userId)
        {
            var result = new Result<List<PaymentResponseDTO>>();

            try
            {
                var payments = await _context.Payments
                    .Where(p => p.Userid == userId)
                    .ToListAsync();

                result.Data = payments.Select(p => new PaymentResponseDTO
                {
                    Id = p.Id,
                    UserId = p.Userid,
                    FineId = p.Fineid,
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

        public async Task<Result<PaymentResponseDTO>> GetPaymentByIdAsync(int paymentId)
        {
            var result = new Result<PaymentResponseDTO>();

            try
            {
                var payment = await _context.Payments.FindAsync(paymentId);
                if (payment == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Payment not found";
                    return result;
                }

                result.Data = new PaymentResponseDTO
                {
                    Id = payment.Id,
                    UserId = payment.Userid,
                    FineId = payment.Fineid,
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

        public async Task<Result<List<PaymentResponseDTO>>> GetUnpaidFinesPaymentsAsync(int userId)
        {
            var result = new Result<List<PaymentResponseDTO>>();

            try
            {
                var unpaidFines = await _context.Fines
                    .Where(f => f.Userid == userId && (f.Paid == null || f.Paid == false))
                    .Select(f => f.Id)
                    .ToListAsync();

                var payments = await _context.Payments
                    .Where(p => unpaidFines.Contains(p.Fineid))
                    .ToListAsync();

                result.Data = payments.Select(p => new PaymentResponseDTO
                {
                    Id = p.Id,
                    UserId = p.Userid,
                    FineId = p.Fineid,
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
