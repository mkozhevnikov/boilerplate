using System.Linq.Expressions;
using System.Text.Json;
using Boilerplate.Common.Data.Querying;
using FluentAssertions;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class OperatorInExpressionTests
{
    [Fact]
    public void OperatorIn_CreateExpression_IntArray_Match()
    {
        var param = Expression.Parameter(typeof(TestValueType));
        var property = Expression.Property(param, nameof(TestValueType.Value));
        var valueParam = Expression.Constant(new[] { 10, 20 });

        var expression = Operator.In.CreateExpression(property, valueParam);

        Expression.Lambda<Func<TestValueType, bool>>(expression, param)
            .Compile()
            .Invoke(new TestValueType(1, 10))
            .Should()
            .BeTrue();
    }

    [Fact]
    public void OperatorIn_CreateExpression_IntArray_NotMatch()
    {
        var param = Expression.Parameter(typeof(TestValueType));
        var property = Expression.Property(param, nameof(TestValueType.Value));
        var valueParam = Expression.Constant(new[] { 10, 20 });

        var expression = Operator.In.CreateExpression(property, valueParam);

        Expression.Lambda<Func<TestValueType, bool>>(expression, param)
            .Compile()
            .Invoke(new TestValueType(1, 30))
            .Should()
            .BeFalse();
    }

    [Fact]
    public void OperatorIn_CreateExpression_StringArray_Match()
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));
        var valueParam = Expression.Constant(new[] { "Foo", "Bar" });

        var expression = Operator.In.CreateExpression(property, valueParam);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue("Foo"))
            .Should()
            .BeTrue();
    }

    [Fact]
    public void OperatorIn_CreateExpression_StringArray_NotMatch()
    {
        var param = Expression.Parameter(typeof(StringValue));
        var property = Expression.Property(param, nameof(StringValue.Value));
        var valueParam = Expression.Constant(new[] { "Foo", "Bar" });

        var expression = Operator.In.CreateExpression(property, valueParam);

        Expression.Lambda<Func<StringValue, bool>>(expression, param)
            .Compile()
            .Invoke(new StringValue("FooBar"))
            .Should()
            .BeFalse();
    }

    private class TestClass
    {
        public object? ValueObject { get; set; }
    }

    [Fact]
    public void OperatorIn_CreateExpression_ValueObject_Match()
    {
        var param = Expression.Parameter(typeof(TestClass));
        var property = Expression.Property(param, nameof(TestClass.ValueObject));
        var valueParam = Expression.Constant(new object[] { 10, 20 });

        var expression = Operator.In.CreateExpression(property, valueParam);

        Expression.Lambda<Func<TestClass, bool>>(expression, param)
            .Compile()
            .Invoke(new TestClass { ValueObject = 10 })
            .Should()
            .BeTrue();
    }

    [Fact]
    public void OperatorIn_CreateExpression_ValueObject_NotMatch()
    {
        var param = Expression.Parameter(typeof(TestClass));
        var property = Expression.Property(param, nameof(TestClass.ValueObject));
        var valueParam = Expression.Constant(new object[] { 10, 20 });

        var expression = Operator.In.CreateExpression(property, valueParam);

        Expression.Lambda<Func<TestClass, bool>>(expression, param)
            .Compile()
            .Invoke(new TestClass { ValueObject = 30 })
            .Should()
            .BeFalse();
    }
}
