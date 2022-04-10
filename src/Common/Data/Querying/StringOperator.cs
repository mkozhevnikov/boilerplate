using System.Linq.Expressions;

namespace Boilerplate.Common.Data.Querying;

public class StringOperator : Operator
{
    private const string startsWith = "startswith";
    public static readonly Operator StartsWith = new StringOperator(startsWith, 9);

    private const string endsWith = "endswith";
    public static readonly Operator EndsWith = new StringOperator(endsWith, 10);

    private const string contains = "contains";
    public static readonly Operator Contains = new StringOperator(contains, 11);

    private const string doesNotContain = "doesnotcontain";
    public static readonly Operator DoesNotContain = new StringOperator(doesNotContain, 12);

    private const string isEmpty = "isempty";
    public static readonly Operator IsEmpty = new StringOperator(isEmpty, 13);

    private const string isNotEmpty = "isnotempty";
    public static readonly Operator IsNotEmpty = new StringOperator(isNotEmpty, 14);

    public StringOperator(string name, int value) : base(name, value)
    {
    }

    public override Expression CreateExpression(Expression left, Expression right) =>
        throw new NotImplementedException();
}
