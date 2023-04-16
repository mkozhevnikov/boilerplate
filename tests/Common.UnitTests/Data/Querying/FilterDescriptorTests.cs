using System.Collections;
using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class FilterDescriptorTests
{
    private record IntComparison(int Value, int CompareTo, Operator Operator);

    private class PositiveIntComparison : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() =>
            IntComparisons().Select(c => new object[] { c.Value, c.CompareTo, c.Operator }).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IEnumerable<IntComparison> IntComparisons()
        {
            yield return new IntComparison(10, 10, Operator.EqualTo);
            yield return new IntComparison(10, 5, Operator.NotEqualTo);
            yield return new IntComparison(10, 20, Operator.LessThan);
            yield return new IntComparison(10, 30, Operator.LessThanOrEqualTo);
            yield return new IntComparison(10, 10, Operator.LessThanOrEqualTo);
            yield return new IntComparison(10, 5, Operator.GreaterThan);
            yield return new IntComparison(10, 1, Operator.GreaterThanOrEqualTo);
            yield return new IntComparison(10, 10, Operator.GreaterThanOrEqualTo);
        }
    }

    private class NegativeIntComparison : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() =>
            IntComparisons().Select(c => new object[] { c.Value, c.CompareTo, c.Operator }).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IEnumerable<IntComparison> IntComparisons()
        {
            yield return new IntComparison(10, 20, Operator.EqualTo);
            yield return new IntComparison(10, 10, Operator.NotEqualTo);
            yield return new IntComparison(10, 5, Operator.LessThan);
            yield return new IntComparison(10, 5, Operator.LessThanOrEqualTo);
            yield return new IntComparison(10, 20, Operator.GreaterThan);
            yield return new IntComparison(10, 20, Operator.GreaterThanOrEqualTo);
        }
    }

    [Theory]
    [ClassData(typeof(PositiveIntComparison))]
    public void FilterDescriptor_ToExpression_IntComparison_Positive(int value, int compareTo, Operator op)
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, value)).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NegativeIntComparison))]
    public void FilterDescriptor_ToExpression_IntComparison_Negative(int value, int compareTo, Operator op)
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, value)).Should().BeFalse();
    }

    private record NullableValue(int? Value);

    private record NullableComparison(int? Value, Operator Operator);

    private class PositiveNullableComparison : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() =>
            NullableComparisons().Select(c => new object[] { c.Value!, c.Operator }).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IEnumerable<NullableComparison> NullableComparisons()
        {
            yield return new NullableComparison(null, Operator.EqualTo);
            yield return new NullableComparison(10, Operator.NotEqualTo);
            yield return new NullableComparison(null, Operator.IsNull);
            yield return new NullableComparison(10, Operator.IsNotNull);
        }
    }

    private class NegativeNullableComparison : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() =>
            NullableComparisons().Select(c => new object[] { c.Value!, c.Operator }).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IEnumerable<NullableComparison> NullableComparisons()
        {
            yield return new NullableComparison(10, Operator.EqualTo);
            yield return new NullableComparison(null, Operator.NotEqualTo);
            yield return new NullableComparison(10, Operator.IsNull);
            yield return new NullableComparison(null, Operator.IsNotNull);
        }
    }

    [Theory]
    [ClassData(typeof(PositiveNullableComparison))]
    public void FilterDescriptor_ToExpression_Nullable_Positive(int? value, Operator op)
    {
        var filter = new FilterDescriptor {
            Field = nameof(NullableValue.Value),
            Operator = op,
            Value = null
        };

        var predicate = filter.ToExpression<NullableValue>();

        predicate.Compile().Invoke(new NullableValue(value)).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NegativeNullableComparison))]
    public void FilterDescriptor_ToExpression_Nullable_Negative(int? value, Operator op)
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = null
        };

        var predicate = filter.ToExpression<NullableValue>();

        predicate.Compile().Invoke(new NullableValue(value)).Should().BeFalse();
    }

    public static IEnumerable<object[]> PositiveStringComparisons => new List<object[]> {
        new object[] { "Foo Bar", "Foo", Operator.StartsWith },
        new object[] { "Foo Bar", "Bar", Operator.EndsWith },
        new object[] { "Foo Bar", "Bar", Operator.Contains },
        new object[] { "Foo Bar", "Baz", Operator.DoesNotContain },
        new object[] { "", null!, Operator.IsEmpty },
        new object[] { null!, null!, Operator.IsEmpty },
        new object[] { "Foo Bar", null!, Operator.IsNotEmpty }
    };

    [Theory]
    [MemberData(nameof(PositiveStringComparisons))]
    public void FilterDescriptor_ToExpression_StringOperations_Positive(
        string value, string? compareTo, Operator op)
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(new StringValue(value)).Should().BeTrue();
    }

    public static IEnumerable<object[]> NegativeStringComparisons => new List<object[]> {
        new object[] { "Foo Bar", "Bar", Operator.StartsWith },
        new object[] { "Foo Bar", "Foo", Operator.EndsWith },
        new object[] { "Foo Bar", "Baz", Operator.Contains },
        new object[] { "Foo Bar", "Bar", Operator.DoesNotContain },
        new object[] { "Foo Bar", null!, Operator.IsEmpty },
        new object[] { "", null!, Operator.IsNotEmpty },
        new object[] { null!, null!, Operator.IsNotEmpty }
    };

    [Theory]
    [MemberData(nameof(NegativeStringComparisons))]
    public void FilterDescriptor_ToExpression_StringOperations_Negative(
        string value, string? compareTo, Operator op)
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(new StringValue(value)).Should().BeFalse();
    }

    [Fact]
    public void FilterDescriptor_ToExpression_String_InArray()
    {
        var filter = new FilterDescriptor {
            Field = nameof(StringValue.Value),
            Operator = Operator.In,
            Value = new[] { "Foo", "Bar", "Foo Bar" }
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(new StringValue("Foo Bar")).Should().BeTrue();
    }

    [Fact]
    public void FilterDescriptor_ToExpression_String_NotInArray()
    {
        var filter = new FilterDescriptor {
            Field = nameof(StringValue.Value),
            Operator = Operator.In,
            Value = new[] { "Foo", "Bar" }
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(new StringValue("Foo Bar")).Should().BeFalse();
    }

    [Fact]
    public void FilterDescriptor_ToExpression_Int_InArray()
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = new[] { 10, 20 }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, 10)).Should().BeTrue();
    }

    [Fact]
    public void FilterDescriptor_ToExpression_Int_NotInArray()
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = new[] { 30, 20 }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, 10)).Should().BeFalse();
    }

    [Theory]
    [InlineData("""{"Field":"Value","Operator":"in","Value":[10,20]}""", 10, true)]
    [InlineData("""{"Field":"Value","Operator":"in","Value":[10,20]}""", 30, false)]
    public void FilterDescriptor_Deserialized_Expression_IntArray(string json, int testValue, bool expected)
    {
        var filter = JsonSerializer.Deserialize<FilterDescriptor>(json);

        var predicate = filter!.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, testValue)).Should().Be(expected);
    }

    [Theory]
    [InlineData("""{"Field":"Value","Operator":"in","Value":["Foo","Bar"]}""", "Foo", true)]
    [InlineData("""{"Field":"Value","Operator":"in","Value":["Foo","Bar"]}""", "FooBar", false)]
    public void FilterDescriptor_Deserialized_ToExpression_StringArray(string json, string testValue, bool expected)
    {
        var filter = JsonSerializer.Deserialize<FilterDescriptor>(json);

        var predicate = filter!.ToExpression<StringValue>();

        predicate.Compile().Invoke(new StringValue(testValue)).Should().Be(expected);
    }

    [Fact]
    public void FilterDescriptor_JsonElementValue_Match()
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = JsonSerializer.SerializeToElement(new[] { 1, 10, 100 })
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, 10)).Should().BeTrue();
    }

    [Fact]
    public void FilterDescriptor_JsonElementValue_NotMatch()
    {
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = JsonSerializer.SerializeToElement(new[] { 1, 10, 100 })
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, 30)).Should().BeFalse();
    }
}
