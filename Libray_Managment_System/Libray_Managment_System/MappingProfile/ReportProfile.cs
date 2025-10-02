using AutoMapper;
using Libray_Managment_System.DtoModels.ReportModels;
using Libray_Managment_System.Models;

namespace Libray_Managment_System.MappingProfile;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Report, ReportResponseDto>();
        CreateMap<ReportCreateDto, Report>();
        CreateMap<ReportUpdateDto, Report>();
    }
}