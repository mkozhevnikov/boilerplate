namespace Boilerplate.Common.Data;

using System.Linq.Expressions;
using Utils;

public static class Specs
{
    public static ISpec<T> True<T>() => new Spec<T>(_ => true);
    public static ISpec<T> False<T>() => new Spec<T>(_ => false);
    public static ISpec<T> And<T>(this ISpec<T> spec, ISpec<T> other) => And(spec, other.Expression);
    public static ISpec<T> And<T>(this ISpec<T> spec, Expression<Func<T, bool>> other) =>
        new Spec<T>(spec.Expression.And(other));
    public static ISpec<T> Or<T>(this ISpec<T> spec, ISpec<T> other) => Or(spec, other.Expression);
    public static ISpec<T> Or<T>(this ISpec<T> spec, Expression<Func<T, bool>> other) =>
        new Spec<T>(spec.Expression.Or(other));
}