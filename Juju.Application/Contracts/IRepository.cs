using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, bool>>[] includes);

        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IReadOnlyList<T> entities);
        void Update(T entity);
        Task Remove(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);

        Task RemoveRange(IEnumerable<T> entities);

    }
}
