using System.Collections;
using System.Linq.Expressions;
using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class FilterDescriptorTests
{
    [Fact]
    public void FilterDescriptor_Serialize_Basic()
    {
        var filter = new FilterDescriptor {
            Field = "name",
            Operator = Operator.EqualTo,
            Value = "John Doe"
        };

        var serializedFilter = JsonConvert.SerializeObject(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Field\":\"{filter.Field}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Operator}\"");
        serializedFilter.Should().Contain($"\"Value\":\"{filter.Value}\"");
    }

    [Fact]
    public void FilterDescriptor_Deserialize_Default()
    {
        var deserializedFilter = JsonConvert.DeserializeObject<FilterDescriptor>(string.Empty);

        deserializedFilter.Should().BeNull();
    }

    [Fact]
    public void FilterDescriptor_Deserialize_Basic()
    {
        var filter = new {
            Field = "name",
            Operator = "eq",
            Value = "John Doe"
        };

        var deserializedFilter = JsonConvert.DeserializeObject<FilterDescriptor>(filter.ToJson());

        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Field.Should().Be(filter.Field);
        deserializedFilter.Operator.Should().Be(Operator.FromName(filter.Operator));
        deserializedFilter.Value.Should().Be(filter.Value);
    }

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
    public void FilterDescriptor_Expression_IntComparison_Positive(int value, int compareTo, Operator op)
    {
        var testValue = new TestValueType(1, value);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NegativeIntComparison))]
    public void FilterDescriptor_Expression_IntComparison_Negative(int value, int compareTo, Operator op)
    {
        var testValue = new TestValueType(1, value);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().BeFalse();
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
    public void FilterDescriptor_Expression_Nullable_Positive(int? value, Operator op)
    {
        var testValue = new NullableValue(value);
        var filter = new FilterDescriptor {
            Field = nameof(NullableValue.Value),
            Operator = op,
            Value = null
        };

        var predicate = filter.ToExpression<NullableValue>();

        predicate.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NegativeNullableComparison))]
    public void FilterDescriptor_Expression_Nullable_Negative(int? value, Operator op)
    {
        var testValue = new NullableValue(value);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = null
        };

        var predicate = filter.ToExpression<NullableValue>();

        predicate.Compile().Invoke(testValue).Should().BeFalse();
    }

    private record StringValue(string Value);

    public static IEnumerable<object[]> PositiveStringComparisons => new List<object[]> {
        new object[] { "Foo Bar", "Foo", StringOperator.StartsWith },
        new object[] { "Foo Bar", "Bar", StringOperator.EndsWith },
        new object[] { "Foo Bar", "Bar", StringOperator.Contains },
        new object[] { "Foo Bar", "Baz", StringOperator.DoesNotContain },
        new object[] { "", null!, StringOperator.IsEmpty },
        new object[] { null!, null!, StringOperator.IsEmpty },
        new object[] { "Foo Bar", null!, StringOperator.IsNotEmpty }
    };

    [Theory]
    [MemberData(nameof(PositiveStringComparisons))]
    public void FilterDescriptor_Expression_StringOperations_Positive(
        string value, string? compareTo, StringOperator op)
    {
        var testValue = new StringValue(value);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(testValue).Should().BeTrue();
    }

    public static IEnumerable<object[]> NegativeStringComparisons => new List<object[]> {
        new object[] { "Foo Bar", "Bar", StringOperator.StartsWith },
        new object[] { "Foo Bar", "Foo", StringOperator.EndsWith },
        new object[] { "Foo Bar", "Baz", StringOperator.Contains },
        new object[] { "Foo Bar", "Bar", StringOperator.DoesNotContain },
        new object[] { "Foo Bar", null!, StringOperator.IsEmpty },
        new object[] { "", null!, StringOperator.IsNotEmpty },
        new object[] { null!, null!, StringOperator.IsNotEmpty }
    };

    [Theory]
    [MemberData(nameof(NegativeStringComparisons))]
    public void FilterDescriptor_Expression_StringOperations_Negative(
        string value, string? compareTo, StringOperator op)
    {
        var testValue = new StringValue(value);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = op,
            Value = compareTo
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(testValue).Should().BeFalse();
    }

    [Fact]
    public void FilterDescriptor_Expression_String_InArray()
    {
        var testValue = new StringValue("Foo Bar");
        var strings = new[] { "Foo", "Bar", "Foo Bar" } as IList;
        strings.Contains(testValue.Value).Should().BeTrue();
        var filter = new FilterDescriptor {
            Field = nameof(StringValue.Value),
            Operator = Operator.In,
            Value = strings
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Fact]
    public void FilterDescriptor_Expression_String_NotInArray()
    {
        var testValue = new StringValue("Foo Bar");
        var filter = new FilterDescriptor {
            Field = nameof(StringValue.Value),
            Operator = Operator.In,
            Value = new[] { "Foo", "Bar" }
        };

        var predicate = filter.ToExpression<StringValue>();

        predicate.Compile().Invoke(testValue).Should().BeFalse();
    }

    [Fact]
    public void FilterDescriptor_Expression_Int_InArray()
    {
        var testValue = new TestValueType(1, 10);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = new[] { 10, 20 }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Fact]
    public void FilterDescriptor_Expression_Int_NotInArray()
    {
        var testValue = new TestValueType(1, 10);
        var filter = new FilterDescriptor {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = new[] { 30, 20 }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().BeFalse();
    }
}
