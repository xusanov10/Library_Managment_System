namespace LibraryManagment.Api.DTOModels
{
    public class OverdueBorrowsResponseDTO
    {
        public int BorrowId { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public decimal FineAmount { get; set; }
    }
}
