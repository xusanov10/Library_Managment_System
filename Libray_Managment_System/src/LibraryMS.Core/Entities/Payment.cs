using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public int Fineid { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Paymentdate { get; set; } = DateTime.UtcNow; 

    public string? Method { get; set; }

    public virtual Fine Fine { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
