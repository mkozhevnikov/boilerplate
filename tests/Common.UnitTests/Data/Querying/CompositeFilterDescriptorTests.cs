using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class CompositeFilterDescriptorTests
{
    [Fact]
    public void CompositeFilterDescriptor_Inheritance_Serialized()
    {
        var filter = new CompositeFilterDescriptor {
            Field = "name",
            Operator = "eq",
            Value = "John Doe"
        };

        var serializedFilter = JsonConvert.SerializeObject(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Field\":\"{filter.Field}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Operator}\"");
        serializedFilter.Should().Contain($"\"Value\":\"{filter.Value}\"");
    }

    [Fact]
    public void CompositeFilterDescriptor_Basic_Serialized()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = "and",
            Filters = new [] {
                new FilterDescriptor {
                    Operator = "eq"
                }
            }
        };

        var serializedFilter = JsonConvert.SerializeObject(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Logic\":\"{filter.Logic}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Filters.Single().Operator}\"");
    }

    [Fact]
    public void CompositeFilterDescriptor_Inheritance_Deserialized()
    {
        var filter = new {
            Field = "name",
            Operator = "eq",
            Value = "John Doe"
        };

        var deserializeFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(filter.ToJson());

        deserializeFilter.Should().NotBeNull();
        deserializeFilter!.Field.Should().Be(filter.Field);
        deserializeFilter.Operator.Should().Be(filter.Operator);
        deserializeFilter.Value.Should().Be(filter.Value);
    }

    [Fact]
    public void CompositeFilterDescriptor_Basic_Deserialized()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = "and",
            Filters = new [] {
                new FilterDescriptor {
                    Operator = "eq"
                }
            }
        };

        var deserializeFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(filter.ToJson());

        deserializeFilter.Should().NotBeNull();
        deserializeFilter!.Logic.Should().Be(filter.Logic);
        deserializeFilter.Filters.Should().HaveCount(1);
        deserializeFilter.Filters.Single().Operator.Should().Be(filter.Filters.Single().Operator);
    }

    [Fact]
    public void CompositeFilterDescriptor_MultiLevel_Deserialized()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = "and",
            Filters = new [] {
                new CompositeFilterDescriptor {
                    Logic = "or",
                    Filters = new [] {
                        new FilterDescriptor {
                            Operator = "eq"
                        }
                    }
                }
            }
        };

        var value = filter.ToJson();
        var deserializeFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(value);

        deserializeFilter.Should().NotBeNull();
        deserializeFilter!.Logic.Should().Be(filter.Logic);
        deserializeFilter.Filters.Should().HaveCount(1);
        var nestedFilter = deserializeFilter.Filters.Single() as CompositeFilterDescriptor;
        nestedFilter.Should().NotBeNull();
        nestedFilter!.Operator.Should().BeNull();
        nestedFilter.Logic.Should().Be(((CompositeFilterDescriptor)filter.Filters.Single()).Logic);
    }
}
