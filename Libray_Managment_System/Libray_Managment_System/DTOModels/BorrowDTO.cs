using System.ComponentModel.DataAnnotations;

namespace Library_Managment_System.DTOModels
{
    public class BorrowDTO
    {
        [Required(ErrorMessage = "UserId majburiy")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "BookCopyId majburiy")]
        public int BookCopyId { get; set; }
        [Required(ErrorMessage = "DueDate majburiy")]
        public DateTime Duedate { get; set; }
    }
}
