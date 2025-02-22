using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<(IEnumerable<BookDto> booksDto,MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
        Task<BookDto?> GetOneBookByIdAsync(int ind, bool trackChanges);    
        Task<BookDto> CreateOneBookAsync(BookDtoInsertion book); 
        Task UpdateOneBookAsync(int id,BookDtoUpdate bookDto, bool trackChanges);  
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<(BookDtoUpdate bookDtoUpdate,Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoUpdate bookDtoUpdate,Book book);

    }
}
