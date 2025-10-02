namespace Libray_Managment_System.DtoModels;

public class AuthorDTO
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Country { get; set; }
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }
}