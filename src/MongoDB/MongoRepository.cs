using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Common.Data;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Boilerplate.MongoDB
{
    public class MongoRepository<T, TKey> : IRepository<T, TKey>, IAsyncRepository<T, TKey>
        where T : class, IEntity<TKey>
    {
        private readonly ICollectionContext context;
        private IMongoCollection<T> collection = null!;

        public MongoRepository(ICollectionContext context) => this.context = context;

        protected IMongoCollection<T> Collection => collection ??= context.DB.GetCollection<T>(context.Name);

        protected IMongoQueryable<T> Query => Collection.AsQueryable();

        public virtual T? GetById<TK>(TK id) where TK : notnull => Query.SingleOrDefault(d => d.Id.Equals(id));

        public virtual async Task<T?> GetByIdAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull =>
            await Query.SingleOrDefaultAsync(d => d.Id.Equals(id), cancellationToken);

        public virtual T? GetBySpec(ISpec<T>? specification) =>
            Query.FirstOrDefault(specification?.Expression);

        public virtual async Task<T?> GetBySpecAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
            await Query.FirstOrDefaultAsync(specification?.Expression, cancellationToken);

        public virtual IReadOnlyList<T> Read(ISpec<T>? specification) =>
            Query.Where(specification?.Expression).ToList();

        public virtual async Task<IReadOnlyList<T>> ReadAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
            await Query.Where(specification?.Expression).ToListAsync(cancellationToken);

        public virtual int Count(ISpec<T>? specification) =>
            Query.Where(specification?.Expression).Count();

        public virtual async Task<int> CountAsync(ISpec<T>? specification, CancellationToken cancellationToken) =>
            await Query.Where(specification?.Expression).CountAsync(cancellationToken);

        public virtual T Create(T entity)
        {
            Collection.InsertOne(entity, null);
            return entity;
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(entity, null, cancellationToken);
            return entity;
        }

        public virtual T Update(T entity)
        {
            var result = Collection.ReplaceOne(d => d.Id.Equals(entity.Id), entity);
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(entity.Id));
            var result = await Collection.ReplaceOneAsync(expression, entity, default(ReplaceOptions), cancellationToken);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(entity.Id));
            var result = Collection.DeleteOne(expression);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(entity.Id));
            var result = await Collection.DeleteOneAsync(expression, cancellationToken);
        }

        public virtual void Delete<TK>(TK id) where TK : notnull
        {
            var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(id));
            var result = Collection.FindOneAndDelete(expression);
        }

        public virtual async Task DeleteAsync<TK>(TK id, CancellationToken cancellationToken) where TK : notnull
        {
            var expression = new ExpressionFilterDefinition<T>(d => d.Id.Equals(id));
            var result = await Collection.FindOneAndDeleteAsync<T>(expression, default(FindOneAndDeleteOptions<T, T>), cancellationToken);
        }

        public virtual void SaveChanges() => throw new NotImplementedException();

        public virtual Task SaveChangesAsync(CancellationToken cancellationToken) =>
            throw new NotImplementedException();
    }
}
