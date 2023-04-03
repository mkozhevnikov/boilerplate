using System.Text.Json.Serialization;
using Boilerplate.Common.Data.Querying;

namespace Boilerplate.Common.UnitTests.Data.Querying;

internal class TestListRequest : IListRequest
{
}

internal class TestFilteredListRequest : IFilteredListRequest
{
    public FilterDescriptor Filter { get; set; } = null!;
}

internal class TestPagedListRequest : IPagedListRequest
{
    public int Skip { get; set; }
    public int Take { get; set; }
}

internal class TestPagedSortedListRequest : IPagedListRequest, ISortedListRequest
{
    public int Skip { get; set; }
    public int Take { get; set; }
    public ICollection<SortingDescriptor> Sort { get; set; } = null!;
}

internal class TestSortedListRequest : ISortedListRequest
{
    public ICollection<SortingDescriptor> Sort { get; set; } = new List<SortingDescriptor>();

    [JsonConstructor]
    public TestSortedListRequest()
    {
    }

    public TestSortedListRequest(PropertyDescriptor propertyDescriptor, Sort direction)
        : this((propertyDescriptor, direction))
    {
    }

    public TestSortedListRequest(params (PropertyDescriptor propertyDescriptor, Sort direction)[] sorting)
    {
        Sort = sorting.Select(pair => new SortingDescriptor {
            Property = pair.propertyDescriptor, Direction = pair.direction
        }).ToList();
    }
}
