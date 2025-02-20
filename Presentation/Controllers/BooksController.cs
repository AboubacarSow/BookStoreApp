using Entities.DataTransfertObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(IServiceManager _manager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
           var models = await _manager.BookService.GetAllBooksAsync(false);
           return Ok(models);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {
           var model =await  _manager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(model);
        }
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
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
        [ServiceFilter(typeof (LogFilterAttribute), Order =2)]
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
            var result= await _manager.BookService.GetOneBookForPatchAsync(id, false);

            //Pour pouvoir specifier ModelState dans cette methode ApplyTo() il faudra install le package 
            //Microsoft.AspNetCore.NewtonsoftJson 
            //  Ces deux methodes capturent toutes des éventuelles erreurs et les stockent dans le ModelState
            bookPatch.ApplyTo(result.bookDtoUpdate,ModelState);
            TryValidateModel(result.bookDtoUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoUpdate,result.book);
            return NoContent(); 
        }

    }
}
