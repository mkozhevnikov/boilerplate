namespace Boilerplate.Common.Data;

public interface IAsyncReadRepository<T, TR>
{
    Task<TR?> GetByIdAsync<TK>(TK id, CancellationToken cancellationToken = default) where TK : notnull;
    Task<TR?> GetBySpecAsync(ISpec<T> specification, CancellationToken cancellationToken = default);
    Task<IList<TR>> ReadAsync(ISpec<T>? specification = null, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpec<T>? specification = null, CancellationToken cancellationToken = default);
}

public interface IAsyncBaseRepository<T, TR> : IAsyncReadRepository<T, TR>
{
    Task<TR> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<TR> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync<TK>(TK key, CancellationToken cancellationToken = default) where TK : notnull;
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IAsyncRepository<T, TKey> : IAsyncBaseRepository<T, T> where T : IEntity<TKey>
{
}
