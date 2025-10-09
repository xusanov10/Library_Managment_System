using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Userprofile
{
    public int Id { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public virtual User IdNavigation { get; set; } = null!;
}
