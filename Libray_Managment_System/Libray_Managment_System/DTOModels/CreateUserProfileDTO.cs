namespace Libray_Managment_System.DtoModels
{
    public class CreateUserProfileDTO
    {
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }
        public int UserId { get; set; }
    }
}
