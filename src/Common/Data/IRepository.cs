namespace Boilerplate.Common.Data;

public interface IReadRepository<T, TR>
{
    TR? GetById<TK>(TK id) where TK : notnull;
    TR? GetBySpec(ISpec<T> specification);
    IReadOnlyList<TR> Read(ISpec<T>? specification = null);
    int Count(ISpec<T>? specification = null);
}

public interface IBaseRepository<T, TR> : IReadRepository<T, TR>
{
    TR Create(T entity);
    TR Update(T entity);
    void Delete(T entity);
    void Delete<TK>(TK key) where TK : notnull;
    void SaveChanges();
}

public interface IRepository<T, TKey> : IBaseRepository<T, T> where T : IEntity<TKey>
{}