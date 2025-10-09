using System.ComponentModel.DataAnnotations;

namespace Libray_Managment_System.DTOModels
{
    public class PaymentDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int FineId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "The payment amount must be greater than 0")]
        public decimal Amount { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
    }
}
