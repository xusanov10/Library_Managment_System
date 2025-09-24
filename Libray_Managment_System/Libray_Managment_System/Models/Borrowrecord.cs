using Libray_Managment_System.Enum;
using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Borrowrecord
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public int Bookcopyid { get; set; }

    public DateTime? Borrowdate { get; set; }

    public DateTime Duedate { get; set; }

    public DateTime? Returndate { get; set; }
    public BorrowStatus Status { get; set; } = BorrowStatus.Borrowed;
    public decimal? Penalty { get; set; }
    public virtual Bookcopy Bookcopy { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual User User { get; set; } = null!;
}
