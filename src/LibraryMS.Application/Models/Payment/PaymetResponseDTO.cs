namespace LibraryMS.Application.Models.Payment
{
    public class PaymetResponseDTO
    {
        public int Id { get; set; }             
        public int UserId { get; set; }             
        public int FineId { get; set; }         
        public decimal Amount { get; set; }        
        public string PaymentMethod { get; set; }   
        public DateTime PaymentDate { get; set; }  
    }
}
