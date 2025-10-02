using Libray_Managment_System.Enum;

namespace Libray_Managment_System.DtoModels.ReportModels;

public class ReportResponseDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Generatedby { get; set; }
    public DateTime? Generatedat { get; set; }
    public ReportType Reporttype { get; set; }
    public string? Filepath { get; set; }
}