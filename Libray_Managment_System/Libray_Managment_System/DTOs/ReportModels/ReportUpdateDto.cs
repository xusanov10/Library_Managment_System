using Libray_Managment_System.Enum;

namespace Libray_Managment_System.DtoModels.ReportModels;

public class ReportUpdateDto
{
    public string? Title { get; set; }
    public int Generatedby { get; set; }
    public ReportType Reporttype { get; set; } = ReportType.Daily;
    public string? Filepath { get; set; }
}