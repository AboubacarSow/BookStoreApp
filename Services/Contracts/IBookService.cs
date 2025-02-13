using Entities.DataTransfertObjects;
using Entities.Models;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto? GetOneBookById(int ind, bool trackChanges);    
        BookDto CreateOneBook(BookDtoInsertion book);  
        void UpdateOneBook(int id,BookDtoUpdate bookDto, bool trackChanges);  
        void DeleteOneBook(int id, bool trackChanges);
        (BookDtoUpdate bookDtoUpdate,Book book) GetOneBookForPatch(int id, bool trackChanges);
        void SaveChangesForPatch(BookDtoUpdate bookDtoUpdate,Book book);

    }
}
