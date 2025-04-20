using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;

namespace Repositories.EFCore
{
    public class CategoryRepository : RepositoryBase<Category>,ICategoryRepository
    {
      

        public CategoryRepository(RepositoryContext context) : base(context)
        {
            
        }

        public void CreateOneCategory(Category category) => Create(category);


        public void DeleteOneCategory(Category category) => Delete(category);
       

        public async Task<PagedList<Category>> GetAllCategoriesAsync(CategoryParameters categoryParameters, bool trackChanges)
        {
            var source = await FindAll(trackChanges)
                             .Search(categoryParameters.SearchTerm!)
                             .Sort(categoryParameters.OrderBy!)
                            .ToListAsync();

            return PagedList<Category>
                .ToPagedList(source, categoryParameters.PageNumber, categoryParameters.PageSize);
        }

        public async Task<Category?> GetOneCategoryByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(c => c.CategoryId.Equals(id), trackChanges)
                .SingleOrDefaultAsync();


        public void UpdateOneCategory(Category category) => Update(category);
        
    }
}
