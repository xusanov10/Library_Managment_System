namespace Libray_Managment_System.DTOModels
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FineId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}
