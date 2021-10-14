using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Boilerplate.Common.Data
{
    public interface IAsyncReadRepository<T, TR>
    {
        Task<TR?> GetByIdAsync<TK>(TK id, CancellationToken cancellationToken = default) where TK : notnull;
        Task<TR?> GetBySpecAsync(ISpec<T> specification, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TR>> Read(ISpec<T>? specification = null, CancellationToken cancellationToken = default);
    }

    public interface IAsyncBaseRepository<T, TR> : IAsyncReadRepository<T, TR>
    {
        Task<TR> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<TR> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync<TK>(TK key, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IAsyncRepository<T, TKey> : IAsyncBaseRepository<T, T> where T : IEntity<TKey>
    {}
}