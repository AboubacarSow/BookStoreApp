using Entities.DataTransfertObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(IServiceManager _manager) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
           var models = _manager.BookService.GetAllBooks(false);
           return Ok(models);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
           var model = _manager.BookService.GetOneBookById(id, false);
           if (model is null)
                throw new BookNotFoundException(id);
            return Ok(model);
        }
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] BookDtoInsertion book)
        {          
           if (book is null)
               return BadRequest();
           if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
           var model = _manager.BookService.CreateOneBook(book);
           return StatusCode(201, model);          
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute] int id, [FromBody] BookDtoUpdate book)
        {            
            if (id != book.id)
                throw new BookBadRequestException("Not match founded wiht two ID");
            if(book is null)
                throw new BookBadRequestException("Book is Null");//400
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);//404
           _manager.BookService.UpdateOneBook(id, book, true);
           return Ok(book);           
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            return NoContent();     
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute] int id, [FromBody] JsonPatchDocument<BookDtoUpdate> bookPatch)
        {
            if (bookPatch is null)
                throw new BookBadRequestException("Book is Null");
            var result= _manager.BookService.GetOneBookForPatch(id, false);

            //Pour pouvoir specifier ModelState dans cette methode ApplyTo() il faudra install le package 
            //Microsoft.AspNetCore.NewtonsoftJson 
            bookPatch.ApplyTo(result.bookDtoUpdate,ModelState);

            //Quel est le role de cette methode
            TryValidateModel(result.bookDtoUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _manager.BookService.SaveChangesForPatch(result.bookDtoUpdate,result.book);
           // _manager.BookService.UpdateOneBook(id,result.bookDtoUpdate,true);
            return NoContent(); 
        }

    }
}
