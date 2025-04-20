using Entities.Models;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class CategoryRepositoryExtension
    {
        public static IQueryable<Category> Search(this IQueryable<Category> categories, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
                return categories;
            string lowerCaseTerm = searchTerm.Trim().ToLower();
            return categories.Where(c => c.CategoryName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Category> Sort(this IQueryable<Category> categories, string orderQueryString)
        {
            if (String.IsNullOrWhiteSpace(orderQueryString))
                return categories.OrderBy(c => c.CategoryId);
            var orderQuery = OrderQueryBuilder
                .CreateOrderQuery<Category>(orderQueryString);

            return orderQuery is null
                ? categories.OrderBy(c => c.CategoryId)
                : categories.OrderBy(orderQuery);
        }
    }
}
