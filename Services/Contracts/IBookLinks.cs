using Microsoft.AspNetCore.Http;
using Entities.LinkModels;
using Entities.DataTransfertObjects.BookDtos;
namespace Services.Contracts
{
    public interface IBookLinks{
        LinkResponse TryGenerateLinks(IEnumerable<BookDto> bookDto,string fields,HttpContext httpContextcontext);
    }
    
}