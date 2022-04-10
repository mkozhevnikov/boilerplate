using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class CompositeFilterDescriptorTests
{
    [Fact]
    public void CompositeFilterDescriptor_Serialize_Basic()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new [] {
                new FilterDescriptor {
                    Operator = Operator.EqualTo
                }
            }
        };

        var serializedFilter = JsonConvert.SerializeObject(filter);

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

        var serializedFilter = JsonConvert.SerializeObject(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Field\":\"{filter.Field}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Operator}\"");
        serializedFilter.Should().Contain($"\"Value\":\"{filter.Value}\"");
    }

    [Fact]
    public void CompositeFilterDescriptor_Deserialize_Default()
    {
        var deserializeFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(string.Empty);

        deserializeFilter.Should().BeNull();
    }

    [Fact]
    public void CompositeFilterDescriptor_Deserialize_Basic()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new [] {
                new FilterDescriptor {
                    Operator = Operator.EqualTo
                }
            }
        };

        var deserializedFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(filter.ToJson());

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

        var deserializedFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(filter.ToJson());

        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Field.Should().Be(filter.Field);
        deserializedFilter.Operator.Should().Be(Operator.FromName(filter.Operator));
        deserializedFilter.Value.Should().Be(filter.Value);
    }

    [Fact]
    public void CompositeFilterDescriptor_Deserialize_MultiLevel()
    {
        var filter = new CompositeFilterDescriptor {
            Logic = Logic.And,
            Filters = new [] {
                new CompositeFilterDescriptor {
                    Logic = Logic.Or,
                    Filters = new [] {
                        new FilterDescriptor {
                            Operator = Operator.EqualTo
                        }
                    }
                }
            }
        };

        var value = filter.ToJson();
        var deserializedFilter = JsonConvert.DeserializeObject<CompositeFilterDescriptor>(value);

        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(filter.Logic);
        deserializedFilter.Filters.Should().HaveCount(1);
        var nestedFilter = deserializedFilter.Filters.Single() as CompositeFilterDescriptor;
        nestedFilter.Should().NotBeNull();
        nestedFilter!.Operator.Should().BeNull();
        nestedFilter.Logic.Should().Be(((CompositeFilterDescriptor)filter.Filters.Single()).Logic);
    }
}
