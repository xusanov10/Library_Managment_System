using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMS.Application.Models.Notification
{
    public class NotificationResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
