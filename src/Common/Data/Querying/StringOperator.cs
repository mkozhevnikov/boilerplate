using System.Linq.Expressions;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.Data.Querying;

public abstract partial class Operator
{
    private const string startsWith = "startswith";
    private const string endsWith = "endswith";
    private const string contains = "contains";
    private const string doesNotContain = "doesnotcontain";
    private const string isEmpty = "isempty";
    private const string isNotEmpty = "isnotempty";

    public static readonly Operator StartsWith = new StartsWithOperator();
    public static readonly Operator EndsWith = new EndsWithOperator();
    public static readonly Operator Contains = new ContainsOperator();
    public static readonly Operator DoesNotContain = new DoesNotContainOperator();
    public static readonly Operator IsEmpty = new IsEmptyOperator();
    public static readonly Operator IsNotEmpty = new IsNotEmptyOperator();

    private sealed class StartsWithOperator : Operator
    {
        public StartsWithOperator() : base(startsWith, 9) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            StringExpression.StartsWith(property, value);
    }

    private sealed class EndsWithOperator : Operator
    {
        public EndsWithOperator() : base(endsWith, 10) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            StringExpression.EndsWith(property, value);
    }

    private sealed class ContainsOperator : Operator
    {
        public ContainsOperator() : base(contains, 11) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            StringExpression.Contains(property, value);
    }

    private sealed class DoesNotContainOperator : Operator
    {
        public DoesNotContainOperator() : base(doesNotContain, 12) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.Not(StringExpression.Contains(property, value));
    }

    private sealed class IsEmptyOperator : Operator
    {
        public IsEmptyOperator() : base(isEmpty, 13) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            StringExpression.IsEmpty(property);
    }

    private sealed class IsNotEmptyOperator : Operator
    {
        public IsNotEmptyOperator() : base(isNotEmpty, 14) {}

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.Not(StringExpression.IsEmpty(property));
    }
}
