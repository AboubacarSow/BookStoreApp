﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
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
