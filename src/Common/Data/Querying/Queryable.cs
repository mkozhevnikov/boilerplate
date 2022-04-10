using System.ComponentModel;
using System.Linq.Expressions;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.Data.Querying;

public static class Queryable
{
    public static IQueryable<T> Sort<T>(
        this IQueryable<T> query,
        ListSortDescriptionCollection sorting)
    {
        if (sorting.Count == 0) {
            return query;
        }

        return sorting.AsEnumerable<ListSortDescription>()
            .Aggregate(query, (q, sort) => q == query ? q.OrderBy(sort) : q.ThenBy(sort));
    }

    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> query,
        ListSortDescription sort) =>
        query.OrderBy(sort.PropertyDescriptor, sort.SortDirection == ListSortDirection.Ascending
            ? nameof(System.Linq.Queryable.OrderBy)
            : nameof(System.Linq.Queryable.OrderByDescending));

    public static IQueryable<T> ThenBy<T>(
        this IQueryable<T> query,
        ListSortDescription sort) =>
        query.OrderBy(sort.PropertyDescriptor, sort.SortDirection == ListSortDirection.Ascending
            ? nameof(System.Linq.Queryable.ThenBy)
            : nameof(System.Linq.Queryable.ThenByDescending));

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

        return query.Provider.CreateQuery<T>(Expression.Call(
            typeof(System.Linq.Queryable),
            sortMethod,
            new[] { componentType, propertyDescriptor.PropertyType },
            query.Expression, lambda
        ));
    }
}
