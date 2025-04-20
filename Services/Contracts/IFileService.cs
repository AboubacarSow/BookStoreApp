using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services.Contracts
{
    public interface IFileService
    {
        Task<String> Upload(IFormFile file);
        Task<(byte[] bytes, string contentTyp, string filePath)> Download(string fileName);
    }
}
