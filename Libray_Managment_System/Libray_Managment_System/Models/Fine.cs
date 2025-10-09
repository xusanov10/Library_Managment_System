using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Fine
{
    public int Id { get; set; }
    public int UserId { get; internal set; }
    public int BorrowRecordId { get; set; }

    public decimal Amount { get; set; }

    public bool? Paid { get; set; }

    public DateTime? Createdat { get; set; } = DateTime.UtcNow;

    public virtual Borrowrecord Borrowrecord { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
