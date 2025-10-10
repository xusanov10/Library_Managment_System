using System.ComponentModel.DataAnnotations;

namespace Library_Managment_System.DTOModels
{
    public class PaymentDTO
    {

        [Required]
        public int UserId { get; set; }
        [Required]
        public int FineId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Tolov summasi 0 dan katta bolishi kerak")]
        public decimal Amount { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
    }
}
