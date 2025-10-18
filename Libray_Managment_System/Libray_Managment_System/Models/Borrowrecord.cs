using Libray_Managment_System.Enum;

namespace Libray_Managment_System.Models;

public partial class Borrowrecord
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public int Bookcopyid { get; set; }

    public DateTime? Borrowdate { get; set; } = DateTime.UtcNow;

    public DateTime Duedate { get; set; } = DateTime.UtcNow.AddDays(14);

    public DateTime? Returndate { get; set; }
    public BorrowStatus Status { get; set; } = BorrowStatus.Borrowed;
    public decimal? Penalty { get; set; }
    public virtual Bookcopy Bookcopy { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual User User { get; set; } = null!;
}
