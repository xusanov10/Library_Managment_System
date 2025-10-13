using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Contactinfo { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
