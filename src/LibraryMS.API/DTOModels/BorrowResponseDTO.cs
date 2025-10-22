using Libray_Managment_System.Enum;

namespace Libray_Managment_System.DtoModels
{
    public class BorrowResponseDTO
    {
        public int BorrowRecordId { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public BorrowStatus Status { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
