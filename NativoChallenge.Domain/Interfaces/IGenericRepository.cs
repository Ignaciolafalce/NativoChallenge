using System.Linq.Expressions;

namespace NativoChallenge.Domain.Interfaces;

public interface IGenericRepository<T, TId> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task DeleteAsync(TId id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
}
