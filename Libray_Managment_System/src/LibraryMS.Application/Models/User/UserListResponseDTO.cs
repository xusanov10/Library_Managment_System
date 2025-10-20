using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMS.Application.Models.User
{
    public class UserListResponseDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool? Status { get; set; }

        public DateTime? Createdat { get; set; }
    }
}
