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
}
