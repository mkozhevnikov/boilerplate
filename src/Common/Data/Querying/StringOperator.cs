using System.Linq.Expressions;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.Data.Querying;

public abstract class StringOperator : Operator
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

    public override Expression CreateExpression(Expression property, Expression value)
    {
        var method = typeof(string).GetMethod(MethodName, new[] { typeof(string) });
        return Expression.Call(property, method, value);
    }

    protected abstract string MethodName { get; }

    protected StringOperator(string name, int value) : base(name, value)
    {
    }

    private sealed class StartsWithOperator : StringOperator
    {
        public StartsWithOperator() : base(startsWith, 9) {}

        protected override string MethodName => nameof(string.StartsWith);
    }

    private sealed class EndsWithOperator : StringOperator
    {
        public EndsWithOperator() : base(endsWith, 10) {}

        protected override string MethodName => nameof(string.EndsWith);
    }

    private sealed class ContainsOperator : StringOperator
    {
        public ContainsOperator() : base(contains, 11) {}

        protected override string MethodName => nameof(string.Contains);
    }

    private sealed class DoesNotContainOperator : StringOperator
    {
        public DoesNotContainOperator() : base(doesNotContain, 12) {}

        protected override string MethodName => nameof(string.Contains);

        public override Expression CreateExpression(Expression property, Expression value) =>
            Expression.Not(base.CreateExpression(property, value));
    }

    private sealed class IsEmptyOperator : StringOperator
    {
        public IsEmptyOperator() : base(isEmpty, 13) {}

        protected override string MethodName => nameof(StringExtensions.IsEmpty);

        public override Expression CreateExpression(Expression property, Expression value)
        {
            var method = typeof(StringExtensions).GetMethod(MethodName, new[] { typeof(string) });
            return Expression.Call(null, method, property);
        }
    }

    private sealed class IsNotEmptyOperator : StringOperator
    {
        public IsNotEmptyOperator() : base(isNotEmpty, 14) {}

        protected override string MethodName => nameof(StringExtensions.IsNotEmpty);

        public override Expression CreateExpression(Expression property, Expression value)
        {
            var method = typeof(StringExtensions).GetMethod(MethodName, new[] { typeof(string) });
            return Expression.Call(null, method, property);
        }
    }
}
