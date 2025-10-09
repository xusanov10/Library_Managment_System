namespace Library_Managment_System.Services.Payment
{
    public interface IPaymentService
    {
        Task MakePaymentAsync(PaymentDto dto);
        Task<List<PaymentResponseDto>> GetUserPaymentsAsync(int userId);
        Task<PaymentResponseDto> GetPaymentByIdAsync(int paymentId);
        Task<List<PaymentResponseDto>> GetUnpaidFinesPaymentsAsync(int userId);
    }
}
