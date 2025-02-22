using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface IBookRepository :IRepositoryBase<Book>
    {
        //Adding Pagination Features

        Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges);
        Task<Book?> GetOneBookByIdAsync(int id,bool trackChanges);
        void CreateOneBook(Book book);  
        void UpdateOneBook(Book book);  
        void DeleteOneBook(Book book);
    }
}
