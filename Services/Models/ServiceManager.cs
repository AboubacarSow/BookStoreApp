using AutoMapper;
using Entities.DataTransfertObjects;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Models
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;

        public ServiceManager(IRepositoryManager manager, ILoggerService logger,
         IMapper mapper,IDataShaper<BookDto> shaper,IBookLinks bookLinks)
        {
            _bookService= new Lazy<IBookService>(()=>new BookManager(manager,logger,mapper,shaper,bookLinks));    
        }

        public IBookService BookService => _bookService.Value;
    }
}
