using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();
}
