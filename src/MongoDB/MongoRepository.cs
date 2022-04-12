using Boilerplate.Common.Data;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Boilerplate.MongoDB;

public class MongoRepository<T, TKey> : IRepository<T, TKey>, IAsyncRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    private readonly ICollectionContext context;

    public MongoRepository(ICollectionContext context) => this.context = context;

    private IMongoCollection<T>? collection;
    protected IMongoCollection<T> Collection => collection ??= context.DB.GetCollection<T>(context.Name);

    protected IMongoQueryable<T> Query => Collection.AsQueryable();

    public virtual T? GetById<TK>(TK id) where TK : notnull =>
        Query.SingleOrDefault(d => d.Id.Equals(id));

    public virtual async Task<T?> GetByIdAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull =>
        await Query.SingleOrDefaultAsync(d => d.Id.Equals(id), cancellationToken);

    public virtual T? GetBySpec(ISpec<T> specification) =>
        Query.Sort(specification).FirstOrDefault(specification.Expression);

    public virtual async Task<T?> GetBySpecAsync(ISpec<T> specification, CancellationToken cancellationToken) =>
        await Query.Sort(specification).FirstOrDefaultAsync(specification.Expression, cancellationToken);

    private IMongoQueryable<T> ReadInternal(ISpec<T>? specification)
    {
        if (specification is null) {
            return Query;
        }

        var query = Query.Where(specification.Expression);

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
            ? Query.Count()
            : Query.Where(specification.Expression).Count();

    public virtual async Task<int> CountAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
        specification is null
            ? await Query.CountAsync(cancellationToken)
            : await Query.Where(specification.Expression).CountAsync(cancellationToken);

    public virtual T Create(T entity)
    {
        Collection.InsertOne(entity);
        return entity;
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await Collection.InsertOneAsync(entity, null, cancellationToken);
        return entity;
    }

    public virtual T Update(T entity)
    {
        Collection.ReplaceOne(d => d.Id.Equals(entity.Id), entity);
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(entity.Id));
        await Collection.ReplaceOneAsync(expression, entity, default(ReplaceOptions), cancellationToken);
        return entity;
    }

    public virtual void Delete(T entity)
    {
        var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(entity.Id));
        Collection.DeleteOne(expression);
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(entity.Id));
        await Collection.DeleteOneAsync(expression, cancellationToken);
    }

    public virtual void Delete<TK>(TK id) where TK : notnull
    {
        var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(id));
        Collection.FindOneAndDelete(expression);
    }

    public virtual async Task DeleteAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull
    {
        var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(id));
        await Collection.FindOneAndDeleteAsync<T>(expression, null, cancellationToken);
    }

    public virtual void SaveChanges() =>
        throw new NotImplementedException();

    public virtual Task SaveChangesAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
