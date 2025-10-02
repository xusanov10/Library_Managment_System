using AutoMapper;
using Libray_Managment_System.DtoModels.AuthorModels;
using Libray_Managment_System.Models;

namespace Libray_Managment_System.MappingProfile;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorResponseDto>();
        CreateMap<AuthorCreateDto, Author>();
        CreateMap<AuthorUpdateDto, Author>();
    }
}