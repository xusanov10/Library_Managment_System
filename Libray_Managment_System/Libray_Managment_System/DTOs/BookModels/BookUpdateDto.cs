namespace Libray_Managment_System.DTOs.BookModels;

public class BookUpdateDto
{
    public string Title { get; set; } = null!;
    public int Authorid { get; set; }
    public int Publisherid { get; set; }
    public int Categoryid { get; set; }
    public string? Isbn { get; set; }
    public int? Quantity { get; set; }
    public string? Filepath { get; set; }
    public string? Publishedyear { get; set; }
}