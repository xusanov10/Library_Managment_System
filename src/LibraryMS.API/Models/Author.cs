namespace Libray_Managment_System.Models;

public partial class Author
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Country { get; set; }

    public string? Birthyear { get; set; }

    public string? Deathyear { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
