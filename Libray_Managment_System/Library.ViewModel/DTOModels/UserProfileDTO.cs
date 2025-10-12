using Library_Managment_System.Enum;

namespace Libray_Managment_System.DTOModels
{
    public class UserProfileDTO
    {
        public string? Phonenumber { get; set; }

        public string? Address { get; set; }

        public DateOnly? Birthdate { get; set; }

        public GenderEnum? Gender { get; set; } 

        public string? ProfilePictureUrl { get; set; } // fayl URL manzili

    }
}
