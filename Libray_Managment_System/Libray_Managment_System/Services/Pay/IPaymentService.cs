using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Pay
{
    public interface IPaymentService
    {
        Task<ResultDTO<string>> MakePaymentAsync(PaymentDto dto);
        Task<ResultDTO<List<PaymentResponseDto>>> GetUserPaymentsAsync(int userId);
        Task<ResultDTO<PaymentResponseDto>> GetPaymentByIdAsync(int paymentId);
        Task<ResultDTO<List<PaymentResponseDto>>> GetUnpaidFinesPaymentsAsync(int userId);
    }
}
