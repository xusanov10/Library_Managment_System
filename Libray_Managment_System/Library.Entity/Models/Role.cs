using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
