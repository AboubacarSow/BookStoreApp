using Entities.DataTransfertObjects.CategoryDtos;
using Entities.Exceptions;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    public class CategoriesController:ControllerBase
    {
        private readonly IServiceManager _service;
        public CategoriesController(IServiceManager service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet(Name ="GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]CategoryParameters categoryParameters)
        {
            var (models,metaDta)= await _service.CategoryService.GetAllCategoriesAsync(categoryParameters,false);
            Response.Headers["X-Pagination"] =
                JsonSerializer.Serialize(metaDta);
            return Ok(models);
        }

        [Authorize]
        [HttpGet("{id:int}",Name ="GetOneCategory")]
        public async Task<IActionResult> GetOneCategory([FromRoute]int id)
        {
            var model = await _service.CategoryService.GetOneCategoryByIdAsync(id, false);
            return Ok(model);
        }

        [Authorize(Roles ="Editor,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name ="CreateOneCategory")]
        public async Task<IActionResult> CreateOneCategory([FromBody] CategoryForInsertionDto categoryDto)
        {
            await _service.CategoryService.CreateOneBookAsync(categoryDto);
            return Ok(new { 
                statusCode = 201,
                Message = "One category item is created" 
            });
        }

        [Authorize(Roles ="Editor,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}",Name ="UpdateOneCategory")]
        public async Task<IActionResult> UpdateOneCategory([FromRoute] int id,
            [FromBody]CategoryForUpdateDto categoryDto)
        {
            if (id != categoryDto.CategoryId)
                throw new CategoryBadRequestException(id.ToString());
            await _service.CategoryService.UpdateOneCategoryAsync(id, categoryDto, true);
            return Ok(new { 
                StatusCode = 200, 
                Message = $"Category item with id:{id} is successfully updated"
            });
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneCategory([FromRoute]int id)
        {
            if (String.IsNullOrWhiteSpace(id.ToString()))
                throw new CategoryBadRequestException("The given id is null or empty");
            await _service.CategoryService.DeleteOneCategoryAsync(id,true);
            return Ok(new {
                StatusCode=200,
                Message=$"Category item with id:{id} is successfully deleted"
            });
        }


    }
}
