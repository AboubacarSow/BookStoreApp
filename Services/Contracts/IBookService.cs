using System.Dynamic;
using Entities.DataTransfertObjects.BookDtos;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<(LinkResponse linkResponse,MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, bool trackChanges);
        Task<BookDto?> GetOneBookByIdAsync(int ind, bool trackChanges);    
        Task<BookDto> CreateOneBookAsync(BookDtoInsertion book); 
        Task UpdateOneBookAsync(int id,BookDtoUpdate bookDto, bool trackChanges);  
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<(BookDtoUpdate bookDtoUpdate,Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoUpdate bookDtoUpdate,Book book);
        Task<List<BookDto>> GetAllBooksAsync(bool trackChanges);
    }
}
