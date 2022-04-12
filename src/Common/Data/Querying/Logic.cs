using System.Linq.Expressions;
using Ardalis.SmartEnum;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.Data.Querying;

public abstract class Logic : SmartEnum<Logic>
{
    private const string or = nameof(or);
    private const string and = nameof(and);

    public static readonly Logic Or = new OrLogic();
    public static readonly Logic And = new AndLogic();

    public abstract Expression<Func<T, bool>> CreateExpression<T>(
        Expression<Func<T, bool>> left, Expression<Func<T, bool>> right);

    protected Logic(string name, int value) : base(name, value)
    {
    }

    private sealed class OrLogic : Logic
    {
        public OrLogic() : base(or, 1) {}

        public override Expression<Func<T, bool>> CreateExpression<T>(
            Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) =>
            left.Or(right);
    }

    private sealed class AndLogic : Logic
    {
        public AndLogic() : base(and, 2) {}

        public override Expression<Func<T, bool>> CreateExpression<T>(
            Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) =>
            left.And(right);
    }
}
