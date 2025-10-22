using Libray_Managment_System.Enum;

namespace LibraryMS.Application.Models.Borrow
{
    public class BorrowResponseDTO
    {
        public int BorrowRecordId { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public BorrowStatus Status { get; set; }
    }
}
