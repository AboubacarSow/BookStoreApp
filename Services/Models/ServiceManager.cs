using AutoMapper;
using Entities.DataTransfertObjects.BookDtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Models
{
    public class ServiceManager : IServiceManager
    {
        private readonly IBookService _bookService;
        private readonly IAuthenticationService _authService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;

        public ServiceManager(IBookService bookService, 
            ICategoryService categoryService,
            IAuthenticationService authService,
            IFileService fileService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authService = authService;
            _fileService = fileService;
        }

        public IBookService BookService => _bookService;
        public IAuthenticationService AuthenticationService => _authService;

        public ICategoryService CategoryService => _categoryService;

        public IFileService FileService => _fileService;
    }
}
