using Entities.DataTransfertObjects;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Entities.LinkModels;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Routing;

namespace Services.Models
{
    public class BookLinks : IBookLinks
    {
        private readonly IDataShaper<BookDto> _dataShaper;
        public BookLinks(IDataShaper<BookDto> dataShaper)
        {
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> bookDto, string fields, HttpContext httpContext)
        {
           var shapedBooks= ShapeData(bookDto,fields);
           if(ShouldGenerateLink(httpContext))
              return ReturnLinkedBooks(bookDto,fields, httpContext,shapedBooks);
              return ReturShapedBooks(shapedBooks);
        }

        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> bookDto, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
           var bookList= bookDto.ToList();
           for(var index=0;index< bookList.Count; index++){
                var booklinks=CreateForBook(httpContext,bookList[index],fields);
                shapedBooks[index].Add("Links",booklinks);
           }
            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            var result=CreateForBooks(httpContext,bookCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = result};
        }

        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext, LinkCollectionWrapper<Entity> bookCollectionWrapper)
        {
            bookCollectionWrapper.Links.Add(new Link() 
            {
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString()!.ToLower()}",
                Relation = "self",
                Method = "GET"
            });
            return bookCollectionWrapper;
        }

        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>()
            {
               new()
               { 
                   Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString()!.ToLower()}" +
                   $"/{bookDto.id}",
                   Relation = "self",
                   Method = "GET"
               },
               new()
               {
                   Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString()!.ToLower()}",
                   Relation="create",
                   Method = "POST"
               },
               new()
               {
                   Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString()!.ToLower()}/{bookDto.id}",
                   Relation="update",
                   Method = "PUT"
               },
                new()
                {
                   Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString()!.ToLower()}/{bookDto.id}",
                   Relation="delete",
                   Method = "POST"
                }
            };
            return links;
        }

        private LinkResponse ReturShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse(){ShapedEntities=shapedBooks};
        }

        private bool ShouldGenerateLink(HttpContext httpContext)
        {
            var mediaType=(MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType
            .SubTypeWithoutSuffix
            .EndsWith("hateoas",StringComparison.CurrentCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<BookDto> bookDtos,string fields)
        {
            return _dataShaper.ShapeData(bookDtos,fields).Select(b=>b.Entity).ToList();
        }
    }
    
}