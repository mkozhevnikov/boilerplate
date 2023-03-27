using System.Text.Json;
using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Xunit;
using Enumerable = System.Linq.Enumerable;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class CompositeFilterDescriptorTests
{
    [Fact]
    public void CompositeFilterDescriptor_Serialize_Basic()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new[] {
                new FilterDescriptor {
                    Operator = Operator.EqualTo
                }
            }
        };

        var serializedFilter = JsonSerializer.Serialize<FilterDescriptor>(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Logic\":\"{filter.Logic}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Filters.Single().Operator}\"");
    }

    [Fact]
    public void CompositeFilterDescriptor_Serialize_Inheritance()
    {
        var filter = new CompositeFilterDescriptor {
            Field = "name",
            Operator = Operator.EqualTo,
            Value = "John Doe"
        };

        var serializedFilter = JsonSerializer.Serialize<FilterDescriptor>(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Field\":\"{filter.Field}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Operator}\"");
        serializedFilter.Should().Contain($"\"Value\":\"{filter.Value}\"");
    }

    [Fact]
    public void CompositeFilterDescriptor_Deserialize_Basic()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new[] {
                new FilterDescriptor {
                    Operator = Operator.EqualTo
                }
            }
        };

        var deserializedFilter = JsonSerializer.Deserialize<CompositeFilterDescriptor>(filter.ToJson());

        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(filter.Logic);
        deserializedFilter.Filters.Should().HaveCount(1);
        deserializedFilter.Filters.Single().Operator.Should().Be(filter.Filters.Single().Operator);
    }

    [Fact]
    public void CompositeFilterDescriptor_Deserialize_Inheritance()
    {
        var filter = new {
            Field = "name",
            Operator = "eq",
            Value = "John Doe"
        };

        var deserializedFilter = JsonSerializer.Deserialize<CompositeFilterDescriptor>(filter.ToJson());

        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Field.Should().Be(filter.Field);
        deserializedFilter.Operator.Should().Be(Operator.FromName(filter.Operator));
        deserializedFilter.Value!.ToString().Should().Be(filter.Value);
    }

    [Fact]
    public void CompositeFilterDescriptor_Deserialize_MultiLevel()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new[] {
                new CompositeFilterDescriptor {
                    Logic = Logic.Or,
                    Filters = new[] {
                        new FilterDescriptor {
                            Operator = Operator.EqualTo
                        }
                    }
                }
            }
        };
        var serializedFilter = filter.ToJson();

        var deserializedFilter = JsonSerializer.Deserialize<CompositeFilterDescriptor>(serializedFilter);

        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(filter.Logic);
        deserializedFilter.Filters.Should().HaveCount(1);
        var nestedFilter = deserializedFilter.Filters.Single() as CompositeFilterDescriptor;
        nestedFilter.Should().NotBeNull();
        nestedFilter!.Operator.Should().BeNull();
        nestedFilter.Logic.Should().Be(((CompositeFilterDescriptor)filter.Filters.Single()).Logic);
    }

    [Fact]
    public void CompositeFilterDescriptor_Expression_NoDescriptors()
    {
        var filter = new CompositeFilterDescriptor {
            Filters = Enumerable.Empty<FilterDescriptor>()
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(new TestValueType(1, 100)).Should().BeTrue();
    }

    [Fact]
    public void CompositeFilterDescriptor_Expression_OneDescriptor_Positive()
    {
        var testValue = new TestValueType(1, 10);
        var filter = new CompositeFilterDescriptor {
            Filters = new[] {
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.EqualTo,
                    Value = testValue.Value
                }
            }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().BeTrue();
    }

    [Fact]
    public void CompositeFilterDescriptor_Expression_OneDescriptor_Negative()
    {
        var testValue = new TestValueType(1, 10);
        var filter = new CompositeFilterDescriptor {
            Filters = new[] {
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.EqualTo,
                    Value = 5
                }
            }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().BeFalse();
    }

    [Theory]
    [InlineData(10, 5, 20, true)]
    [InlineData(10, 5, 10, false)]
    [InlineData(10, 15, 20, false)]
    [InlineData(10, 15, 10, false)]
    public void CompositeFilterDescriptor_Expression_And(int value, int greaterThan, int lessThan, bool expected)
    {
        var testValue = new TestValueType(1, value);
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new[] {
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.GreaterThan,
                    Value = greaterThan
                },
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.LessThan,
                    Value = lessThan
                }
            }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().Be(expected);
    }

    [Theory]
    [InlineData(10, 5, 20, true)]
    [InlineData(10, 5, 10, true)]
    [InlineData(10, 15, 20, true)]
    [InlineData(10, 15, 10, false)]
    public void CompositeFilterDescriptor_Expression_Or(int value, int greaterThan, int lessThan, bool expected)
    {
        var testValue = new TestValueType(1, value);
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.Or,
            Filters = new[] {
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.GreaterThan,
                    Value = greaterThan
                },
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.LessThan,
                    Value = lessThan
                }
            }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().Be(expected);
    }

    [Fact]
    public void CompositeFilterDescriptor_Expression_MultiLevel()
    {
        var testValue = new TestValueType(1, 10);
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.Or,
            Filters = new[] {
                new FilterDescriptor {
                    Field = nameof(TestValueType.Value),
                    Operator = Operator.EqualTo,
                    Value = 5
                },
                new CompositeFilterDescriptor {
                    Logic = Logic.Or,
                    Filters = new[] {
                        new FilterDescriptor {
                            Field = nameof(TestValueType.Value),
                            Operator = Operator.GreaterThan,
                            Value = 5
                        },
                        new FilterDescriptor {
                            Field = nameof(TestValueType.Value),
                            Operator = Operator.LessThan,
                            Value = 20
                        }
                    }
                }
            }
        };

        var predicate = filter.ToExpression<TestValueType>();

        predicate.Compile().Invoke(testValue).Should().Be(true);
    }
}
