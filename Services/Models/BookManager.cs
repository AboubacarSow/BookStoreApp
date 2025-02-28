using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Exceptions;
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
        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
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
            var modelDto=await GetOneBookByIdAsync(id,trackChanges);
            _manager.Book.DeleteOneBook(_mapper.Map<Book>(modelDto));
            await _manager.SaveAsync();
        }

        //Why is it necessary to return a tuple in this level?
        public async Task<(IEnumerable<BookDto>,MetaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            if (!bookParameters.ValidePriceRange)
                throw new PriceOutOfRangeBadRequestException();
            var booksWithPagedList=await _manager.Book.GetAllBooksAsync(bookParameters,trackChanges);
           var bookDto=  _mapper.Map<IEnumerable<BookDto>>(booksWithPagedList);
            return (bookDto, booksWithPagedList.MetaData);
        }

        public async Task<BookDto?> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var model = await GetOneBookByIdAndCheckExist(id, trackChanges);
            return _mapper.Map<BookDto>(model);
        }

        private async Task<Book?> GetOneBookByIdAndCheckExist(int id, bool trackChanges)
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
            var model=await GetOneBookByIdAndCheckExist(id,trackChanges);
            model=_mapper.Map<Book>(bookDto);
            _manager.Book.Update(model);
            await _manager.SaveAsync();
        }
    }
}
