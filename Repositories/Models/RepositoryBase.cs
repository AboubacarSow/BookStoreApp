using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq.Expressions;

namespace Repositories.Models
{
    //THE BASE CLASS MUST BE AN ABSTRACT CLASS
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
           _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity); 
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? 
                _context.Set<T>().AsNoTracking():
                _context.Set<T>();  
        }

        public T? FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges)
        {
            return !trackChanges ?
                   _context.Set<T>().Where(condition).AsNoTracking().SingleOrDefault():
                   _context.Set<T>().Where(condition).SingleOrDefault();   
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);   
        }
    }
}
