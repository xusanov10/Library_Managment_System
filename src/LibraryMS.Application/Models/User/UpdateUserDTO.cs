namespace LibraryMS.Application.Models.User 
{ 
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool? Status { get; set; }
    }
}
