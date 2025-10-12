using Library_Managment_System.Enum;
using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Userprofile
{
    public int Id { get; set; }

    public string? Phonenumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? ProfilePictureUrl { get; set; } // MinIO URL

    public GenderEnum? Gender { get; set; } 

    public virtual User IdNavigation { get; set; } = null!;
}
