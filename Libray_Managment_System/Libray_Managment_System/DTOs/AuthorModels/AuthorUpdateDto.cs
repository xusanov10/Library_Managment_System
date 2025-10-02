namespace Libray_Managment_System.DtoModels.AuthorModels;

public class AuthorUpdateDto
{
    public string Fullname { get; set; } = null!;
    public string? Country { get; set; }
    public string? Birthyear { get; set; }
    public string? Deathyear { get; set; }
}