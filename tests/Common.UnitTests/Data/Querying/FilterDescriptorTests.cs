using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class FilterDescriptorTests
{
    [Fact]
    public void FilterDescriptor_Basic_Serialized()
    {
        var filter = new FilterDescriptor {
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
    public void FilterDescriptor_Basic_Deserialized()
    {
        var filter = new {
            Field = "name",
            Operator = "eq",
            Value = "John Doe"
        };

        var deserializeFilter = JsonConvert.DeserializeObject<FilterDescriptor>(filter.ToJson());

        deserializeFilter.Should().NotBeNull();
        deserializeFilter!.Field.Should().Be(filter.Field);
        deserializeFilter.Operator.Should().Be(filter.Operator);
        deserializeFilter.Value.Should().Be(filter.Value);
    }
}
