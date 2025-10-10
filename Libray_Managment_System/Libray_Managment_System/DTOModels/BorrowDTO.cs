using System.ComponentModel.DataAnnotations;

namespace Library_Managment_System.DTOModels
{
    public class BorrowDTO
    {
        public int UserId { get; set; }       
        public int BookCopyId { get; set; }   
    }
}
