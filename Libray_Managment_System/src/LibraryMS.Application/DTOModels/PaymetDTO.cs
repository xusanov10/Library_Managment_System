using System.ComponentModel.DataAnnotations;

namespace Library_Managment_System.DTOModels
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
