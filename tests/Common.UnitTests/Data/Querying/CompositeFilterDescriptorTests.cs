using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class CompositeFilterDescriptorTests
{
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
