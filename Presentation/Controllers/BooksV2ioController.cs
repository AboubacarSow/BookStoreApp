using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiExplorerSettings(GroupName ="v2")]
    [ApiController]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/books")]
    public class BooksV2ioController(IServiceManager manager) : ControllerBase
    {
        private readonly IServiceManager _manager = manager;

        /// <summary>
        /// Retrieves all books with pagination, sorting, and HATEOAS links.
        /// </summary>
        /// <param name="bookParameters">Pagination, sorting, and filtering parameters</param>
        /// <returns>A paginated list of books with or without links based on the header</returns>
        /// <response code="200">Returns the list of books</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _manager.BookService.GetAllBooksAsync(false);
            var bookV2 = books.Select(b => new 
            {
               id = b.id,
                title=b.Title
            }).ToList();

            return Ok(bookV2);
        }
    }
}
