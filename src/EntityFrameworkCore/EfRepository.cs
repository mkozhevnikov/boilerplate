namespace Boilerplate.EntityFrameworkCore;

using Common.Data;
using Microsoft.EntityFrameworkCore;

public class EfRepository<T, TKey> : IRepository<T, TKey>, IAsyncRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    private readonly DbContext _context;

    public EfRepository(DbContext context) => _context = context;

    public virtual T? GetById<TK>(TK id) where TK : notnull => _context.Set<T>().Find(id);

    public virtual async Task<T?> GetByIdAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull =>
        await _context.Set<T>().FindAsync(id, cancellationToken);

    public virtual T? GetBySpec(ISpec<T>? specification) =>
        _context.Set<T>().FirstOrDefault(specification?.Expression);

    public virtual async Task<T?> GetBySpecAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
        await _context.Set<T>().FirstOrDefaultAsync(specification?.Expression, cancellationToken);

    public virtual IReadOnlyList<T> Read(ISpec<T>? specification) =>
        _context.Set<T>().Where(specification?.Expression).ToList();

    public virtual async Task<IReadOnlyList<T>> ReadAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
        await _context.Set<T>().Where(specification?.Expression).ToListAsync(cancellationToken);

    public virtual int Count(ISpec<T>? specification) =>
        _context.Set<T>().Where(specification?.Expression).Count();

    public virtual async Task<int> CountAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
        await _context.Set<T>().Where(specification?.Expression).CountAsync(cancellationToken);

    public virtual T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        SaveChanges();
        return entity;
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual T Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        SaveChanges();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        SaveChanges();
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public virtual void Delete<TK>(TK id) where TK : notnull
    {
        var entity = GetById(id);
        if (entity is null) return;
        _context.Set<T>().Remove(entity);
        SaveChanges();
    }

    public virtual async Task DeleteAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null) return;
        _context.Set<T>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public void SaveChanges() => _context.SaveChanges();

    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await _context.SaveChangesAsync(cancellationToken);
}
