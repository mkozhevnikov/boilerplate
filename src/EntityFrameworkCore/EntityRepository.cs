using Boilerplate.Common.Data;
using Boilerplate.Common.Data.Querying;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.EntityFrameworkCore;

public class EntityRepository<T, TKey> : IRepository<T, TKey>, IAsyncRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    private readonly DbContext context;

    public EntityRepository(DbContext context) => this.context = context;

    public virtual T? GetById<TK>(TK id) where TK : notnull => context.Set<T>().Find(id);

    public virtual async Task<T?> GetByIdAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull =>
        await context.Set<T>().FindAsync(id, cancellationToken);

    public virtual T? GetBySpec(ISpec<T> specification) =>
        context.Set<T>().Where(specification.Expression).Sort(specification).FirstOrDefault();

    public virtual async Task<T?> GetBySpecAsync(ISpec<T> specification, CancellationToken cancellationToken) =>
        await context.Set<T>()
            .Where(specification.Expression)
            .Sort(specification)
            .FirstOrDefaultAsync(cancellationToken);

    private IQueryable<T> ReadInternal(ISpec<T>? specification)
    {
        if (specification is null) {
            return context.Set<T>();
        }

        var query = context.Set<T>().Where(specification.Expression);

        if (specification.Skip is not null) {
            query = query.Skip(specification.Skip.Value);
        }

        if (specification.Take is not null) {
            query = query.Take(specification.Take.Value);
        }

        return query.Sort(specification);
    }

    public virtual IList<T> Read(ISpec<T>? specification) =>
        ReadInternal(specification).ToList();

    public virtual async Task<IList<T>> ReadAsync(ISpec<T>? specification,
        CancellationToken cancellationToken = default) =>
        await ReadInternal(specification).ToListAsync(cancellationToken);

    public virtual int Count(ISpec<T>? specification) =>
        specification is null
            ? context.Set<T>().Count()
            : context.Set<T>().Where(specification.Expression).Count();

    public virtual async Task<int> CountAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
        specification is null
            ? await context.Set<T>().CountAsync(cancellationToken)
            : await context.Set<T>().Where(specification.Expression).CountAsync(cancellationToken);

    public virtual T Create(T entity)
    {
        context.Set<T>().Add(entity);
        SaveChanges();
        return entity;
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual T Update(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        SaveChanges();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        SaveChanges();
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public virtual void Delete<TK>(TK id) where TK : notnull
    {
        var entity = GetById(id);
        if (entity is null) {
            return;
        }

        context.Set<T>().Remove(entity);
        SaveChanges();
    }

    public virtual async Task DeleteAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null) {
            return;
        }

        context.Set<T>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public void SaveChanges() => context.SaveChanges();

    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);
}
