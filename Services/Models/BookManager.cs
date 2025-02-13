using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Exceptions;
using Entities.Models;
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

        public BookDto CreateOneBook(BookDtoInsertion bookDtoInsertion)
        {
            var book = _mapper.Map<Book>(bookDtoInsertion);
           _manager.Book.CreateOneBook(book);
            _manager.Save();
            return _mapper.Map<BookDto>(book);    
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var modelDto=GetOneBookById(id,trackChanges);
            _manager.Book.DeleteOneBook(_mapper.Map<Book>(modelDto));
            _manager.Save();
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books=_manager.Book.GetAllBooks(trackChanges);
            return  _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto? GetOneBookById(int id, bool trackChanges)
        {
            var model = _manager.Book.GetOneBookById(id, trackChanges);
            
            if (model is null)
            {
                string msg= $"Book with id:{id} could not be founded";
                _logger.LogInfo(msg);
                throw new BookNotFoundException(id);
            }
            return _mapper.Map<BookDto>(model);
        }

        public (BookDtoUpdate bookDtoUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id, trackChanges);
            if (book is null)
                throw new BookNotFoundException(id);
            var bookDtoUpdate=_mapper.Map<BookDtoUpdate>(book);   
            return (bookDtoUpdate, book);   
        }

        public void SaveChangesForPatch(BookDtoUpdate bookDtoUpdate, Book book)
        {
           _mapper.Map(bookDtoUpdate, book);
            _manager.Save();
        }

        public void UpdateOneBook(int id, BookDtoUpdate bookDto, bool trackChanges)
        {
            var model=_manager.Book.GetOneBookById(id,trackChanges);
            model=_mapper.Map<Book>(bookDto);
            _manager.Book.Update(model);
            _manager.Save();
        }
    }
}
