using Library_Managment_System.Enum;
using Microsoft.AspNetCore.Http;

namespace Libray_Managment_System.DtoModels
{
    public class CreateUserProfileDTO
    {
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly? BirthDate { get; set; }
        public GenderEnum? Gender { get; set; }
        public IFormFile? ProfilePicture { get; set; } // yangi qo'shildi
        public int UserId { get; set; }
    }
}
