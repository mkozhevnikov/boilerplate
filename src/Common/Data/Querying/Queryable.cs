using System.Linq.Expressions;

namespace Boilerplate.Common.Data.Querying;

public static class Queryable
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> query, ISpec<T> spec) =>
        spec is ISortingSpec<T> sortingSpec ? query.Sort(sortingSpec.Sorting) : query;

    public static IQueryable<T> Sort<T>(
        this IQueryable<T> query,
        ICollection<SortingDescriptor> sorting)
    {
        if (!sorting.Any()) {
            return query;
        }

        return sorting.Aggregate(query, (q, sort) => q.Equals(query)
            ? q.OrderBy(sort.Property, sort.Direction)
            : q.ThenBy(sort.Property, sort.Direction));
    }

    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> query,
        PropertyDescriptor propertyDescriptor,
        Sort sortMethod) =>
        query.OrderBy(propertyDescriptor, sortMethod.OrderByMethod);

    public static IQueryable<T> ThenBy<T>(
        this IQueryable<T> query,
        PropertyDescriptor propertyDescriptor,
        Sort sortMethod) =>
        query.OrderBy(propertyDescriptor, sortMethod.ThenByMethod);

    private static IQueryable<T> OrderBy<T>(
        this IQueryable<T> query,
        PropertyDescriptor propertyDescriptor,
        string sortMethod)
    {
        var componentType = propertyDescriptor.ComponentType;
        if (componentType != typeof(T)) {
            throw new InvalidOperationException("Can't apply ordering. Types mismatch");
        }

        var param = Expression.Parameter(componentType);
        var property = Expression.Property(param, propertyDescriptor.Name);
        var lambda = Expression.Lambda(property, param);

        // represents System.Linq.Queryable.OrderBy<T>((T t) => t.Property)
        return query.Provider.CreateQuery<T>(Expression.Call(
            typeof(System.Linq.Queryable),
            sortMethod,
            new[] { componentType, propertyDescriptor.PropertyType },
            query.Expression, lambda
        ));
    }
}
