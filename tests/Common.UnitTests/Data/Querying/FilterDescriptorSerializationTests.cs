using Ardalis.SmartEnum.SystemTextJson;
using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class FilterDescriptorSerializationTests
{
    [Fact]
    public void FilterDescriptor_Serialize_Basic()
    {
        var filter = new FilterDescriptor {
            Field = "name",
            Operator = Operator.EqualTo,
            Value = "John Doe"
        };

        var serializedFilter = JsonSerializer.Serialize(filter);

        serializedFilter.Should().NotBeEmpty();
        serializedFilter.Should().Contain($"\"Field\":\"{filter.Field}\"");
        serializedFilter.Should().Contain($"\"Operator\":\"{filter.Operator}\"");
        serializedFilter.Should().Contain($"\"Value\":\"{filter.Value}\"");
    }

    [Fact]
    public void FilterDescriptor_Deserialize_Basic()
    {
        var anonymousObject = new {
            Field = "name",
            Operator = "eq",
            Value = "John Doe"
        };
        var serializedFilter = anonymousObject.ToJson(); // {"Field":"name","Operator":"eq","Value":"John Doe"}

        var filter = JsonSerializer.Deserialize<FilterDescriptor>(serializedFilter);

        filter.Should().NotBeNull();
        filter!.Field.Should().Be(anonymousObject.Field);
        filter.Operator.Should().Be(Operator.FromName(anonymousObject.Operator));
        filter.Value!.ToString().Should().Be(anonymousObject.Value);
    }

    [Fact]
    public void FilterDescriptor_Deserialize_String_Eq()
    {
        var anonymousObject = new {
            Field = nameof(TestValueType.Value),
            Operator = Operator.EqualTo,
            Value = "John Doe"
        };
        var serializedFilter = anonymousObject.ToJson(new SmartEnumNameConverter<Operator, int>());

        var filter = JsonSerializer.Deserialize<FilterDescriptor>(serializedFilter);

        filter.Should().NotBeNull();
        filter!.Field.Should().Be(anonymousObject.Field);
        filter.Operator.Should().Be(anonymousObject.Operator);
        filter.Value!.ToString().Should().Be(anonymousObject.Value);
    }

    [Fact]
    public void FilterDescriptor_Deserialize_Int_InArray()
    {
        var anonymousObject = new {
            Field = nameof(TestValueType.Value),
            Operator = Operator.In,
            Value = new[] { 10, 20 }
        };
        var serializedFilter = anonymousObject.ToJson(new SmartEnumNameConverter<Operator, int>());

        var filter = JsonSerializer.Deserialize<FilterDescriptor>(serializedFilter);

        filter.Should().NotBeNull();
        filter!.Field.Should().Be(anonymousObject.Field);
        filter.Operator.Should().Be(anonymousObject.Operator);

        filter.Value.Should().BeOfType<JsonElement>();
        var value = (JsonElement)filter.Value!;
        value.ValueKind.Should().Be(JsonValueKind.Array);
        value.AsIntArray().Should().Equal(anonymousObject.Value);
    }

    [Fact]
    public void FilterDescriptor_Deserialize_String_InArray()
    {
        var anonymousObject = new {
            Field = nameof(StringValue.Value),
            Operator = Operator.In,
            Value = new[] { "Foo", "Bar" }
        };
        var serializedFilter = anonymousObject.ToJson(new SmartEnumNameConverter<Operator, int>());

        var filter = JsonSerializer.Deserialize<FilterDescriptor>(serializedFilter);

        filter.Should().NotBeNull();
        filter!.Field.Should().Be(anonymousObject.Field);
        filter.Operator.Should().Be(anonymousObject.Operator);

        filter.Value.Should().BeOfType<JsonElement>();
        var value = (JsonElement)filter.Value!;
        value.ValueKind.Should().Be(JsonValueKind.Array);
        value.AsStringArray().Should().Equal(anonymousObject.Value);
    }
}
