using System.Linq.Expressions;
using System.Text.Json;
using Ardalis.SmartEnum;
using Boilerplate.Common.Utils;

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
    private const string contains = "in";

    public static readonly Operator EqualTo = new EqualToOperator();
    public static readonly Operator NotEqualTo = new NotEqualToOperator();
    public static readonly Operator IsNull = new IsNullOperator();
    public static readonly Operator IsNotNull = new IsNotNullOperator();
    public static readonly Operator LessThan = new LessThanOperator();
    public static readonly Operator LessThanOrEqualTo = new LessThanOrEqualToOperator();
    public static readonly Operator GreaterThan = new GreaterThanOperator();
    public static readonly Operator GreaterThanOrEqualTo = new GreaterThanOrEqualToOperator();
    public static readonly Operator In = new ContainsOperator();

    public abstract Expression CreateExpression(Expression property, Expression value);

    protected Operator(string name, int value) : base(name, value) {}

    private sealed class EqualToOperator : Operator
    {
        public EqualToOperator() : base(equalTo, 1) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.Equal(property, value);
    }

    private sealed class NotEqualToOperator : Operator
    {
        public NotEqualToOperator() : base(notEqual, 2) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.NotEqual(property, value);
    }

    private sealed class IsNullOperator : Operator
    {
        public IsNullOperator() : base(isNull, 3) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.Equal(property, Expression.Constant(null, typeof(object)));
    }

    private sealed class IsNotNullOperator : Operator
    {
        public IsNotNullOperator() : base(isNotNull, 4) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.NotEqual(property, Expression.Constant(null, typeof(object)));
    }

    private sealed class LessThanOperator : Operator
    {
        public LessThanOperator() : base(lessThan, 5) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.LessThan(property, value);
    }

    private sealed class LessThanOrEqualToOperator : Operator
    {
        public LessThanOrEqualToOperator() : base(lessThanOrEqualTo, 6) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.LessThanOrEqual(property, value);
    }

    private sealed class GreaterThanOperator : Operator
    {
        public GreaterThanOperator() : base(greaterThan, 7) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.GreaterThan(property, value);
    }

    private sealed class GreaterThanOrEqualToOperator : Operator
    {
        public GreaterThanOrEqualToOperator() : base(greaterThanOrEqualTo, 8) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.GreaterThanOrEqual(property, value);
    }

    private sealed class ContainsOperator : Operator
    {
        public ContainsOperator() : base(contains, 15) {}

        public override Expression CreateExpression(Expression property, Expression value)
        {
            var memberReturnType = property.GetMemberReturnType();
            var filterValue = ((ConstantExpression)value).Value;

            if (filterValue is JsonElement { ValueKind: JsonValueKind.Array } jsonElement) {
                var listFilter = jsonElement.Deserialize(typeof(List<>).MakeGenericType(memberReturnType));
                value = Expression.Constant(listFilter);
            }

            var containsMethod = typeof(Enumerable).GetMethods()
                .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(memberReturnType);
            return Expression.Call(containsMethod, value, property);
        }
    }
}
