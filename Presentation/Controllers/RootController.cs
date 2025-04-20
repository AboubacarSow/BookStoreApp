using Entities.LinkModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (!mediaType.Contains("application/vnd.btkakademi.apiroot"))
            {
                return NoContent();
            }
            var list = new List<Link>()
            {
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(GetRoot),new{}),
                    Relation="_self",
                    Method="GET"
                },
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(BooksController.GetAllBooks),new{}),
                    Relation="books",
                    Method="GET"

                },
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(BooksController.GetOneBook),new{id=2}),
                    Relation="books",
                    Method="GET"
                },
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(BooksController.CreateOneBook),new{}),
                    Relation="books",
                    Method="POST"
                },
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(BooksController.UpdateOneBook),new{id=2}),
                    Relation="books",
                    Method="PUT"
                },
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(BooksController.PartiallyUpdateOneBook),new{id = 2 }),
                    Relation="books",
                    Method="PATCH"
                },
                new()
                {
                    Href=_linkGenerator.GetUriByName(HttpContext, nameof(BooksController.DeleteOneBook),new{id = 2 }),
                    Relation="books",
                    Method="Delete"
                }
            };
            return Ok(list);

            
        }
    }
}
