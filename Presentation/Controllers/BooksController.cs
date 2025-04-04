using Entities.DataTransfertObjects;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers
{
    //[ApiVersion("1.0")] Through Conventions without attribute ApiVersion
    [Route("api/books")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    public class BooksController(IServiceManager _manager) : ControllerBase
    {
        [HttpHead]
        [HttpGet(Name = "GetAllBooks")]
        [ServiceFilter(typeof(ValidationMediaTypeAttribute))]
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {
           var model =await  _manager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(model);
        }
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name ="CreateOneBook")]
        public async Task<IActionResult> CreateOneBook([FromBody] BookDtoInsertion book)
        {    
            //With our actionfilter ValidationFilterAttribute we no longer need these two if statement
           /*if (book is null)
                 return BadRequest();
             if(!ModelState.IsValid)
                  return UnprocessableEntity(ModelState);*/
           var model = await _manager.BookService.CreateOneBookAsync(book);
           return StatusCode(201, model);          
        }
        [ServiceFilter(type: typeof(ValidationFilterAttribute),Order =1)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBook([FromRoute] int id, [FromBody] BookDtoUpdate book)
        {            
            if (id != book.id)
                throw new BookBadRequestException("Not match founded wiht two ID");
          await  _manager.BookService.UpdateOneBookAsync(id, book, true);
           return Ok(book);           
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();     
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute] int id,
            [FromBody] JsonPatchDocument<BookDtoUpdate> bookPatch)
        {
            if (bookPatch is null)
                throw new BookBadRequestException("Book is Null");
            var (bookDtoUpdate, book) = await _manager.BookService.GetOneBookForPatchAsync(id, false);

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
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers["Allow"] = "GET,POST,PATCH,PUT,DELETE,HEAD,OPTIONS";
            return Ok();
        }

    }
}
