namespace Libray_Managment_System.DtoModels;

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
}