using Entities.DataTransfertObjects.BookDtos;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers
{
    //[ApiVersion("1.0")] Through Conventions without attribute ApiVersion
    //[ResponseCache(CacheProfileName ="3mins")]
    //[HttpCacheExpiration(CacheLocation =CacheLocation.Public,MaxAge =70)]
    // [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/books")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Retrieves all books with pagination, sorting, and HATEOAS links.
        /// </summary>
        /// <param name="bookParameters">Pagination, sorting, and filtering parameters</param>
        /// <returns>A paginated list of books with or without links based on the header</returns>
        /// <response code="200">Returns the list of books</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpHead]
        [HttpGet(Name = "GetAllBooks")]
        [ServiceFilter(typeof(ValidationMediaTypeAttribute))]
        [ResponseCache(Duration =60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetAllBooks([FromQuery]BookParameters bookParameters)
        {
            var linkParameters=new LinkParameters(){
                BookParameters=bookParameters,
                HttpContext=HttpContext
            };
            //We expected two values of reference
           var (linkResponse, metaData) = await _manager.BookService
                                            .GetAllBooksAsync(linkParameters,false);
            Response.Headers["X-Pagination"] = JsonSerializer
            .Serialize(metaData);
           
           
           return linkResponse.HasLinks
                    ? Ok(linkResponse.LinkedEntities)
                    : Ok(linkResponse.ShapedEntities);
                  
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("{id:int}",Name = "GetOneBook")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {
           var model =await  _manager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(model);
        }
        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="bookDto">Data for the book to be created</param>
        /// <returns>The created book</returns>
        /// <response code="201">Book created successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles ="Editor, Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name ="CreateOneBook")]
        public async Task<IActionResult> CreateOneBook([FromBody] BookDtoInsertion book)
        {    
           var model = await _manager.BookService.CreateOneBookAsync(book);
           return StatusCode(201, model);          
        }

        [Authorize(Roles ="Editor, Admin")]
        [ServiceFilter(type: typeof(ValidationFilterAttribute),Order =1)]
        [HttpPut("{id:int}",Name = "UpdateOneBook")]
        public async Task<IActionResult> UpdateOneBook([FromRoute] int id, [FromBody] BookDtoUpdate book)
        {            
            if (id != book.id)
                throw new BookBadRequestException("Not match founded wiht two ID");
          await  _manager.BookService.UpdateOneBookAsync(id, book, true);
           return Ok(book);           
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id:int}",Name = "DeleteOneBook")]
        public async Task<IActionResult> DeleteOneBook([FromRoute] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();     
        }

        [Authorize(Roles ="Admin, Editor")]
        [HttpPatch("{id:int}",Name = "PartiallyUpdateOneBook")]
        public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute] int id,
            [FromBody] JsonPatchDocument<BookDtoUpdate> bookPatch)
        {
            if (bookPatch is null)
                throw new BookBadRequestException("Book is Null");
            var (bookDtoUpdate, book) = await _manager.BookService.GetOneBookForPatchAsync(id, true);

            //Pour pouvoir specifier ModelState dans cette methode ApplyTo() il faudra install le package 
            //Microsoft.AspNetCore.NewtonsoftJson 
            //  Ces deux methodes capturent toutes des éventuelles erreurs et les stockent dans le ModelState
            bookPatch.ApplyTo(bookDtoUpdate,ModelState);
            TryValidateModel(bookDtoUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(bookDtoUpdate,book);
            return NoContent(); 
        }


        [Authorize]
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Allow = "GET,POST,PATCH,PUT,DELETE,HEAD,OPTIONS";
            return Ok();
        }

    }
}
