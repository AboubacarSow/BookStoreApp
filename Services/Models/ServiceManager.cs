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
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<IAuthenticationService> _authService;

        public ServiceManager(IRepositoryManager manager, ILoggerService logger,
         IMapper mapper,IDataShaper<BookDto> shaper,IBookLinks bookLinks,
         UserManager<User> userManager,IConfiguration configuration)
        {
            _bookService= new Lazy<IBookService>(()=>new BookManager(manager,logger,mapper,shaper,bookLinks));
            _authService = new Lazy<IAuthenticationService>(() => new AuthenticationManager(logger, mapper, userManager,configuration));
        }

        public IBookService BookService => _bookService.Value;
        public IAuthenticationService AuthenticationService => _authService.Value;
    }
}
