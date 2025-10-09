using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Message { get; set; }

    public DateTime? Createdat { get; set; } = DateTime.UtcNow; 

    public bool Isread { get; set; }

    public virtual User User { get; set; } = null!;
}
