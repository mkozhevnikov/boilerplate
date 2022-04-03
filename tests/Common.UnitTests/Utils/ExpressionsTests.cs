using System.Linq.Expressions;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Xunit;

namespace Boilerplate.Common.UnitTests.Utils;

public class ExpressionsTests
{
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void Expressions_And_ResultIs(bool first, bool second, bool result)
    {
        Expression<Func<int, bool>> firstExpr = _ => first;
        Expression<Func<int, bool>> secondExpr = _ => second;

        var sut = firstExpr.And(secondExpr);

        sut.Compile().Invoke(1).Should().Be(result);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(false, false, false)]
    public void Expressions_Or_ResultIs(bool first, bool second, bool result)
    {
        Expression<Func<int, bool>> firstExpr = _ => first;
        Expression<Func<int, bool>> secondExpr = _ => second;

        var sut = firstExpr.Or(secondExpr);

        sut.Compile().Invoke(1).Should().Be(result);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Expressions_Not_ResultIs(bool first, bool result)
    {
        Expression<Func<int, bool>> firstExpr = _ => first;

        var sut = firstExpr.Not();

        sut.Compile().Invoke(1).Should().Be(result);
    }
}
