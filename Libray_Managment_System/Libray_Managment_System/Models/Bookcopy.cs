using Libray_Managment_System.Enum;
using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Bookcopy
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int Copynumber { get; set; }
    public BookCopyStatus Status { get; set; } = BookCopyStatus.Available;
    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Borrowrecord> Borrowrecords { get; set; } = new List<Borrowrecord>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
