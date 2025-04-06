using AutoMapper;
using Entities.DataTransfertObjects.BookDtos;
using Entities.DataTransfertObjects.UserDtos;
using Entities.Models;

namespace WebApi.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<BookDtoUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>();
            CreateMap<BookDtoInsertion, Book>();


            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
