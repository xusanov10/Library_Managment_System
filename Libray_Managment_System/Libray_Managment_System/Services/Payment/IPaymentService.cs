using Library_Managment_System.DTOModels;
using Libray_Managment_System.Services;

namespace Library_Managment_System.Services.Payment
{
    public interface IPaymentService
    {
        Task<Result<string>> MakePaymentAsync(PaymentDTO dto);
        Task<Result<List<PaymentResponseDTO>>> GetUserPaymentsAsync(int userId);
        Task<Result<PaymentResponseDTO>> GetPaymentByIdAsync(int paymentId);
        Task<Result<List<PaymentResponseDTO>>> GetUnpaidFinesPaymentsAsync(int userId);
    }
}
