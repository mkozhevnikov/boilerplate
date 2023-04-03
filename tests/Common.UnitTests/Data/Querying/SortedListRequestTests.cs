using System.Text.Json;
using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Xunit;

namespace Boilerplate.Common.UnitTests.Data.Querying;

public class SortedListRequestTests
{
    [Fact]
    public void SortedListRequest_Serialization_Empty()
    {
        var request = new TestSortedListRequest();

        var serialized = request.ToJson();

        serialized.Should().Be("{\"Sort\":[]}");
    }

    [Fact]
    public void SortedListRequest_Serialization_SingleDescriptor()
    {
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var request = new TestSortedListRequest(valuePropertyDescriptor, Sort.Descending);

        var serialized = request.ToJson();

        serialized.Should().Be("{\"Sort\":[{\"Field\":\"Value\",\"Dir\":\"Desc\"}]}");
    }

    [Fact]
    public void SortedListRequest_Serialization_TwoDescriptors()
    {
        var valuePropertyDescriptor = TestPropertyDescriptor.Value;
        var indexPropertyDescriptor = TestPropertyDescriptor.Index;
        var request = new TestSortedListRequest(
            (valuePropertyDescriptor, Sort.Descending),
            (indexPropertyDescriptor, Sort.Ascending)
        );

        var serialized = request.ToJson();

        serialized.Should().Contain("{\"Field\":\"Value\",\"Dir\":\"Desc\"}");
        serialized.Should().Contain("{\"Field\":\"Index\",\"Dir\":\"Asc\"}");
    }

    [Fact]
    public void SortedListRequest_Deserialization_Empty()
    {
        var serializedRequest = "{\"Sort\":[]}";

        var deserialized = JsonSerializer.Deserialize<TestSortedListRequest>(serializedRequest);

        deserialized.Should().NotBeNull();
        deserialized!.Sort.AsEnumerable().Should().BeEmpty();
    }

    [Fact]
    public void SortedListRequest_Deserialization_SingleDescriptor()
    {
        var serializedRequest = "{\"Sort\":[{\"Field\":\"Value\",\"Dir\":\"Desc\"}]}";

        var deserialized = JsonSerializer.Deserialize<TestSortedListRequest>(serializedRequest);

        deserialized.Should().NotBeNull();
        deserialized!.Sort.Should().NotBeEmpty().And.HaveCount(1);
        deserialized.Sort.First().Direction.Should().Be(Sort.Descending);
    }

    [Fact]
    public void SortedListRequest_Deserialization_TwoDescriptors()
    {
        var serializedRequest = "{\"Sort\":[" +
                                "{\"Field\":\"Value\",\"Dir\":\"Desc\"}," +
                                "{\"Field\":\"Index\",\"Dir\":\"Asc\"}]}";

        var deserialized = JsonSerializer.Deserialize<TestSortedListRequest>(serializedRequest);

        deserialized.Should().NotBeNull();
        deserialized!.Sort.Should().NotBeEmpty().And.HaveCount(2);
        deserialized.Sort.First().Direction.Should().Be(Sort.Descending);
        deserialized.Sort.Last().Direction.Should().Be(Sort.Ascending);
    }
}
