using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Application.Models.Payment
{
    public class PaymetDTO
    {
        public int UserId { get; set; }       
        public int FineId { get; set; } 

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }    
        public string PaymentMethod { get; set; } 
    }
}
