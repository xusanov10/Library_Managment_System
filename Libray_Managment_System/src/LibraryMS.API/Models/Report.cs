using Libray_Managment_System.Enum;
using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Report
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int Generatedby { get; set; }

    public DateTime? Generatedat { get; set; } = DateTime.UtcNow;
    public ReportType Reporttype { get; set; } = ReportType.Daily;
    public string? Filepath { get; set; }

    public virtual User GeneratedbyNavigation { get; set; } = null!;
}
