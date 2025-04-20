using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Services.Contracts;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController :ControllerBase
    {
        private readonly IServiceManager _service;

        public FilesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost("upload")]

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filePath=await _service.FileService.Upload(file);
            return Ok(new
            {
                file = file.FileName,
                path = filePath,
                Size = file.Length
            });
        }
       
        [HttpGet("download")]
        public async Task<IActionResult> DownloadAsync(string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // File path  
            var (bytes, contentType, filePath) = await _service.FileService.Download(fileName);

            return File(bytes, contentType, Path.GetFileName(filePath));
        }
    }
}
