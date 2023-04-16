using System.Linq.Expressions;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.Data.Querying;

public static class StringExpression
{
    private static Expression CreateExpression(string methodName, Expression property, Expression value)
    {
        var memberReturnType = property.GetMemberReturnType();
        if (memberReturnType != typeof(string)) {
            throw new ArgumentException($"Can't use string operator with member of Type: {memberReturnType}");
        }

        var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
        return Expression.Call(property, method, value);
    }

    public static Expression StartsWith(Expression property, Expression value) =>
        CreateExpression(nameof(string.StartsWith), property, value);

    public static Expression EndsWith(Expression property, Expression value) =>
        CreateExpression(nameof(string.EndsWith), property, value);

    public static Expression Contains(Expression property, Expression value) =>
        CreateExpression(nameof(string.Contains), property, value);

    public static Expression IsEmpty(Expression property)
    {
        var method = typeof(string).GetMethod(nameof(string.IsNullOrEmpty), new[] { typeof(string) });
        return Expression.Call(null, method, property);
    }
}
