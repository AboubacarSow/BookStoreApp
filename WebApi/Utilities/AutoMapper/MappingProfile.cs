using AutoMapper;
using Entities.DataTransfertObjects.BookDtos;
using Entities.DataTransfertObjects.CategoryDtos;
using Entities.DataTransfertObjects.UserDtos;
using Entities.Models;

namespace WebApi.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //BookMapper
            CreateMap<BookDtoUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>();
            CreateMap<BookDtoInsertion, Book>();

            //UserMapper
            CreateMap<UserForRegistrationDto, User>();


            //CategoryMapper
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryForInsertionDto, Category>();
            CreateMap<CategoryForUpdateDto, Category>();
        }
    }
}
