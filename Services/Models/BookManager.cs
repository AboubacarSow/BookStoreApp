using System.Dynamic;
using System.Runtime.CompilerServices;
using AutoMapper;
using Entities.DataTransfertObjects.BookDtos;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Models
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<BookDto> _shaper;
        private readonly IBookLinks _bookLinks;
        public BookManager(IRepositoryManager manager, ILoggerService logger,
        IMapper mapper,IDataShaper<BookDto> shaper,IBookLinks bookLinks)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _shaper=shaper;
            _bookLinks=bookLinks;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoInsertion bookDtoInsertion)
        {
            var book = _mapper.Map<Book>(bookDtoInsertion);
            _manager.Book.CreateOneBook(book);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(book);    
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var modelDto=await GetOneBookByIdAndCheckExist(id,trackChanges);
            _manager.Book.DeleteOneBook(_mapper.Map<Book>(modelDto));
            await _manager.SaveAsync();
        }

        //Why is it necessary to return a tuple in this level?
        public async Task<(LinkResponse linkResponse,MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, bool trackChanges)
        {
            if (!linkParameters.BookParameters.ValidePriceRange)
                throw new PriceOutOfRangeBadRequestException();
            var booksWithPagedList=await _manager.Book.GetAllBooksAsync(linkParameters.BookParameters,trackChanges);
           var bookDto=  _mapper.Map<IEnumerable<BookDto>>(booksWithPagedList);
           var links= _bookLinks.TryGenerateLinks(bookDto,
           linkParameters.BookParameters.Fields!,linkParameters.HttpContext);
           
            return (linkResponse: links, metaData: booksWithPagedList.MetaData);
        }

        public async Task<BookDto?> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var model = await GetOneBookByIdAndCheckExist(id, trackChanges);
            return _mapper.Map<BookDto>(model);
        }

        private async Task<Book> GetOneBookByIdAndCheckExist(int id, bool trackChanges)
        {
            var model = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (model is null)
            {
                string msg = $"Book with id:{id} could not be founded";
                _logger.LogInfo(msg);
                throw new BookNotFoundException(id);
            }

            return model;
        }

        public async Task<(BookDtoUpdate bookDtoUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book =await GetOneBookByIdAndCheckExist(id,trackChanges);
            var bookDtoUpdate=_mapper.Map<BookDtoUpdate>(book);   
            return (bookDtoUpdate, book);   
        }

        public async Task SaveChangesForPatchAsync(BookDtoUpdate bookDtoUpdate, Book book)
        {
           _mapper.Map(bookDtoUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task UpdateOneBookAsync(int id, BookDtoUpdate bookDto, bool trackChanges)
        {
            await GetOneBookByIdAndCheckExist(id,trackChanges);
            var model=_mapper.Map<Book>(bookDto);
            _manager.Book.Update(model);
            await _manager.SaveAsync();
        }

        public async Task<List<BookDto>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _manager.Book.GetAllBooksAsync(trackChanges);
            return _mapper.Map<List<BookDto>>(books);
        }
    }
}
