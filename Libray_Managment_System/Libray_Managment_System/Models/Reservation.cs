using Libray_Managment_System.Enum;
using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookcopyId { get; set; }

    public DateTime? ReservedDate { get; set; } = DateTime.UtcNow;
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public virtual Bookcopy Bookcopy { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
