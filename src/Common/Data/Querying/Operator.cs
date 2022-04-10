using System.Linq.Expressions;
using Ardalis.SmartEnum;

namespace Boilerplate.Common.Data.Querying;

public abstract class Operator : SmartEnum<Operator>
{
    private const string equalTo = "eq";
    private const string notEqual = "neq";
    private const string isNull = "isnull";
    private const string isNotNull = "isnotnull";
    private const string lessThan = "lt";
    private const string lessThanOrEqualTo = "lte";
    private const string greaterThan = "gt";
    private const string greaterThanOrEqualTo = "gte";

    public static readonly Operator EqualTo = new EqualToOperator();
    public static readonly Operator NotEqualTo = new NotEqualToOperator();
    public static readonly Operator IsNull = new IsNullOperator();
    public static readonly Operator IsNotNull = new IsNotNullOperator();
    public static readonly Operator LessThan = new LessThanOperator();
    public static readonly Operator LessThanOrEqualTo = new LessThanOrEqualToOperator();
    public static readonly Operator GreaterThan = new GreaterThanOperator();
    public static readonly Operator GreaterThanOrEqualTo = new GreaterThanOrEqualToOperator();

    public abstract Expression CreateExpression(Expression left, Expression right);

    protected Operator(string name, int value) : base(name, value)
    {
    }

    private sealed class EqualToOperator : Operator
    {
        public EqualToOperator() : base(equalTo, 1) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.Equal(left, right);
    }

    private sealed class NotEqualToOperator : Operator
    {
        public NotEqualToOperator() : base(notEqual, 2) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.NotEqual(left, right);
    }

    private sealed class IsNullOperator : Operator
    {
        public IsNullOperator() : base(isNull, 3) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.Equal(left, Expression.Constant(null, typeof(object)));
    }

    private sealed class IsNotNullOperator : Operator
    {
        public IsNotNullOperator() : base(isNotNull, 4) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.NotEqual(left, Expression.Constant(null, typeof(object)));
    }

    private sealed class LessThanOperator : Operator
    {
        public LessThanOperator() : base(lessThan, 5) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.LessThan(left, right);
    }

    private sealed class LessThanOrEqualToOperator : Operator
    {
        public LessThanOrEqualToOperator() : base(lessThanOrEqualTo, 6) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.LessThanOrEqual(left, right);
    }

    private sealed class GreaterThanOperator : Operator
    {
        public GreaterThanOperator() : base(greaterThan, 7) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.GreaterThan(left, right);
    }

    private sealed class GreaterThanOrEqualToOperator : Operator
    {
        public GreaterThanOrEqualToOperator() : base(greaterThanOrEqualTo, 8) {}

        public override Expression CreateExpression(Expression left, Expression right) =>
            Expression.GreaterThanOrEqual(left, right);
    }
}
