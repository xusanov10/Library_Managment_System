namespace LibraryMS.Application.Models.Fine
{
    public class FineDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BorrowRecordId { get; set; }
        public decimal Amount { get; set; }
        public bool Paid { get; set; }
    }
}
