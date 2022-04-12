using System.Linq.Expressions;

namespace Boilerplate.Common.Data;

public interface ISpec<T>
{
    /// <summary>
    /// Condition to use with Queryable to filter objects satisfying the spec
    /// </summary>
    Expression<Func<T, bool>> Expression { get; }

    /// <summary>
    /// Skipping specification to ignore number of objects
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// Limiting specification to return certain number of objects
    /// </summary>
    int? Take { get; }
}

public class Spec<T> : ISpec<T>
{
    public Expression<Func<T, bool>> Expression { get; }
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public Spec(Expression<Func<T, bool>> expression)
    {
        Expression = expression;
    }
}
