using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public string? Message { get; set; }

    public DateTime? Createdat { get; set; }

    public bool Isread { get; set; }

    public virtual User User { get; set; } = null!;
}
