using Libray_Managment_System.Enum;

namespace Libray_Managment_System.DtoModels;

public class BookCopyDTO
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public BookCopyStatus Status { get; set; }

}