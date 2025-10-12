using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Rolepermission
{
    public int Id { get; set; }

    public int Roleid { get; set; }

    public int Permissionid { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
