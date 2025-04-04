using Entities.DataTransfertObjects;
using Microsoft.AspNetCore.Http;
using Entities.LinkModels;
namespace Services.Contracts
{
    public interface IBookLinks{
        LinkResponse TryGenerateLinks(IEnumerable<BookDto> bookDto,string fields,HttpContext httpContextcontext);
    }
    
}