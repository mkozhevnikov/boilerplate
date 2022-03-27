namespace Boilerplate.Common.Data;

using System.Linq.Expressions;

public interface ISpec<T>
{
    Expression<Func<T, bool>> Expression { get; }
}

public class Spec<T> : ISpec<T>
{
    public Expression<Func<T, bool>> Expression { get; }

    public Spec(Expression<Func<T, bool>> expression)
    {
        Expression = expression;
    }
}