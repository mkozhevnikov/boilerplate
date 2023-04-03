using System.Linq.Expressions;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.Data;

public static class Specs
{
    public static Spec<T> True<T>() => new(_ => true);

    public static Spec<T> False<T>() => new(_ => false);

    public static ISpec<T> And<T>(this ISpec<T> spec, ISpec<T> other) => And(spec, other.Expression);

    public static ISpec<T> And<T>(this ISpec<T> spec, Expression<Func<T, bool>> other) =>
        new Spec<T>(spec.Expression.And(other));

    public static ISpec<T> Or<T>(this ISpec<T> spec, ISpec<T> other) => Or(spec, other.Expression);

    public static ISpec<T> Or<T>(this ISpec<T> spec, Expression<Func<T, bool>> other) =>
        new Spec<T>(spec.Expression.Or(other));
}
