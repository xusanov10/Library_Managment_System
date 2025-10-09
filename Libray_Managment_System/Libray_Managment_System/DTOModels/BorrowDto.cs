using System.ComponentModel.DataAnnotations;

namespace Libray_Managment_System.DTOModels
{
    public class BorrowDto
    {
        [Required(ErrorMessage = "UserId majburiy")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "BookCopyId majburiy")]
        public int BookCopyId { get; set; }
        [Required(ErrorMessage = "DueDate majburiy")]
        public DateTime Duedate { get; set; }
    }
}
