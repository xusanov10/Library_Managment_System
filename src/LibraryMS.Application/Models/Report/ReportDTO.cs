using Libray_Managment_System.Enum;

namespace LibraryMS.Application.Models.Report;

public class ReportDTO
{
    public ReportType Type { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string Data { get; set; }
}