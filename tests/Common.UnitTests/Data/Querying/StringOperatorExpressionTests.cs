using System.Linq.Expressions;
using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class StringOperatorExpressionTests
{
    [Theory]
    [InlineData("Foo", true)]
    [InlineData("Bar", false)]
    public void StringOperator_StartsWith_CreateExpression(string value, bool expected)
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));
        var valueParam = Expression.Constant(value);

        var expression = StringOperator.StartsWith.CreateExpression(property, valueParam);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue("FooBar"))
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("Foo", false)]
    [InlineData("Bar", true)]
    public void StringOperator_EndsWith_CreateExpression(string value, bool expected)
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));
        var valueParam = Expression.Constant(value);

        var expression = StringOperator.EndsWith.CreateExpression(property, valueParam);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue("FooBar"))
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("Foo", true)]
    [InlineData("Bar", true)]
    [InlineData("Test", false)]
    public void StringOperator_Contains_CreateExpression(string value, bool expected)
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));
        var valueParam = Expression.Constant(value);

        var expression = StringOperator.Contains.CreateExpression(property, valueParam);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue("FooBar"))
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("Foo", false)]
    [InlineData("Bar", false)]
    [InlineData("Test", true)]
    public void StringOperator_DoesNotContain_CreateExpression(string value, bool expected)
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));
        var valueParam = Expression.Constant(value);

        var expression = StringOperator.DoesNotContain.CreateExpression(property, valueParam);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue("FooBar"))
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("FooBar", false)]
    [InlineData("", true)]
    public void StringOperator_IsEmpty_CreateExpression(string testValue, bool expected)
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));

        var expression = StringOperator.IsEmpty.CreateExpression(property, null!);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue(testValue))
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("FooBar", true)]
    [InlineData("", false)]
    public void StringOperator_IsNotEmpty_CreateExpression(string testValue, bool expected)
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));

        var expression = StringOperator.IsNotEmpty.CreateExpression(property, null!);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue(testValue))
            .Should()
            .Be(expected);
    }
}
