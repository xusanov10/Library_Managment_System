using Libray_Managment_System.Enum;

namespace Libray_Managment_System.DtoModels;

public class ReportDTO
{
    public ReportType Type { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string Data { get; set; }
}