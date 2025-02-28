using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book)=>Create(book);
        public void DeleteOneBook(Book book)=>Delete(book);

        //Adding Pagination Features
        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
        {

            var source=await FindAll(trackChanges)
                  .FilterBooks(bookParameters.MinPrice,bookParameters.MaxPrice)
                  .Search(bookParameters.SearchTerm)
                  .OrderBy(b=>b.id)
                  .ToListAsync();

            return  PagedList<Book>
                        .ToPagedList(source,bookParameters.PageNumber,bookParameters.PageSize);
        }
        public async Task<Book?> GetOneBookByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.id.Equals(id), trackChanges)
                 .SingleOrDefaultAsync();  
        public void UpdateOneBook(Book book)=>Update(book);
        
    }
}
