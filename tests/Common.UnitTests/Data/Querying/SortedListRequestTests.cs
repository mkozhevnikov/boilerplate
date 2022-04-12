using System.ComponentModel;
using Boilerplate.Common.Data.Querying;
using Boilerplate.Common.Utils;
using FluentAssertions;
using Newtonsoft.Json;
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
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var sortDescription = new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending);
        var request = new TestSortedListRequest(sortDescription);

        var serialized = request.ToJson();

        serialized.Should().Be("{\"Sort\":[{\"Field\":\"Value\",\"Dir\":\"Desc\"}]}");
    }

    [Fact]
    public void SortedListRequest_Serialization_TwoDescriptors()
    {
        var valuePropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Value), null);
        var indexPropertyDescriptor = new TestPropertyDescriptor(nameof(TestValueType.Index), null);
        var sorting = new ListSortDescriptionCollection(new[] {
            new ListSortDescription(valuePropertyDescriptor, ListSortDirection.Descending),
            new ListSortDescription(indexPropertyDescriptor, ListSortDirection.Ascending)
        });
        var request = new TestSortedListRequest(sorting);

        var serialized = request.ToJson();

        serialized.Should().Contain("{\"Field\":\"Value\",\"Dir\":\"Desc\"}");
        serialized.Should().Contain("{\"Field\":\"Index\",\"Dir\":\"Asc\"}");
    }

    [Fact]
    public void SortedListRequest_Deserialization_Empty()
    {
        var serializedRequest = "{\"Sort\":[]}";

        var deserialized = JsonConvert.DeserializeObject<TestSortedListRequest>(serializedRequest);

        deserialized.Should().NotBeNull();
        deserialized!.Sort.AsEnumerable<ListSortDescription>().Should().BeEmpty();
    }

    [Fact]
    public void SortedListRequest_Deserialization_SingleDescriptor()
    {
        var serializedRequest = "{\"Sort\":[{\"Field\":\"Value\",\"Dir\":\"Desc\"}]}";

        var deserialized = JsonConvert.DeserializeObject<TestSortedListRequest>(serializedRequest);

        deserialized.Should().NotBeNull();
        deserialized!.Sort.AsEnumerable<ListSortDescription>().Should().NotBeEmpty().And.HaveCount(1);
        deserialized.Sort[0]!.SortDirection.Should().Be(ListSortDirection.Descending);
    }

    [Fact]
    public void SortedListRequest_Deserialization_TwoDescriptors()
    {
        var serializedRequest = "{\"Sort\":[" +
                                "{\"Field\":\"Value\",\"Dir\":\"Desc\"}," +
                                "{\"Field\":\"Index\",\"Dir\":\"Asc\"}]}";

        var deserialized = JsonConvert.DeserializeObject<TestSortedListRequest>(serializedRequest);

        deserialized.Should().NotBeNull();
        deserialized!.Sort.AsEnumerable<ListSortDescription>().Should().NotBeEmpty().And.HaveCount(2);
        deserialized.Sort[0]!.SortDirection.Should().Be(ListSortDirection.Descending);
        deserialized.Sort[1]!.SortDirection.Should().Be(ListSortDirection.Ascending);
    }

    private class TestSortedListRequest : ISortedListRequest
    {
        [JsonConstructor]
        public TestSortedListRequest(params ListSortDescription[] sortDescriptions)
            : this(new ListSortDescriptionCollection(sortDescriptions))
        {
        }

        public TestSortedListRequest(ListSortDescriptionCollection sorting) => Sort = sorting;

        public ListSortDescriptionCollection Sort { get; set; }
    }
}
