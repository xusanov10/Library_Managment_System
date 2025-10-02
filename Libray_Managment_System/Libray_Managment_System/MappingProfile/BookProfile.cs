using AutoMapper;
using Libray_Managment_System.DTOs.BookModels;
using Libray_Managment_System.Models;

namespace Libray_Managment_System.MappingProfile;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookResponseDto>();
        CreateMap<BookCreateDto, Book>();
        CreateMap<BookUpdateDto, Book>();
    }
}