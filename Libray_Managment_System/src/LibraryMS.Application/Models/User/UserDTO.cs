namespace LibraryMS.Application.Models.User
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool? Status { get; set; }

        public DateTime? Createdat { get; set; }
    }
}
