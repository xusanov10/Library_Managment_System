using AutoMapper;
using Libray_Managment_System.DtoModels.CategoryModels;
using Libray_Managment_System.Models;

namespace Libray_Managment_System.MappingProfile;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponseDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}