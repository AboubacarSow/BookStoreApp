using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Services.Contracts;
using System.IO;
namespace Services.Models
{
    public class FileManager : IFileService
    {
        public async Task<(byte[] bytes,string contentTyp,string filePath)> Download(string fileName) // Changed return type to FileResult
        {
            // File path  
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Medias", fileName);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            var filebytes = await File.ReadAllBytesAsync(path);
            return (bytes:filebytes,contentType,path);
        }

        public async Task<string> Upload(IFormFile file)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Medias");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, file?.FileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return path;
        }
    }
}
