using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Application.Models.Borrow
{
    public class BorrowDTO
    {
        public int UserId { get; set; }       
        public int BookCopyId { get; set; }   
    }
}
