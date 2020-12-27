using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Contracts.Generic
{
    public interface IGenericRepo<T> : IDisposable where T : class
    {
        Task<IQueryable<T>> GetAll();

        Task<List<T>> Find(Expression<Func<T, bool>> predicate);

        Task<T> Add(T entity);

        Task AddRange(IEnumerable<T> entities);


        Task Delete(T entity);

        Task DeleteRange(IEnumerable<T> entities);

        Task<T> Edit(T entity);

        Task EditRange(IEnumerable<T> entities);

        Task<T> GetById(int id);

        T ReloadEntity(T entity);
    }
}
