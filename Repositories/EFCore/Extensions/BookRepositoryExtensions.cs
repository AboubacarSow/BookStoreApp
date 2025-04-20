using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice,uint maxPrice)
        {
            return books.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
        }

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return books;
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return books.Where(b => b.Title.ToLower().Contains(searchTerm));
        }
        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b => b.id);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);
            return orderQuery is null
                   ? books.OrderBy(b => b.id)
                   : books.OrderBy(orderQuery);
        }
    }
}
