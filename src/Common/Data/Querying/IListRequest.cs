using System.ComponentModel;
using Newtonsoft.Json;

namespace Boilerplate.Common.Data.Querying;

public interface IListRequest
{
}

public interface IPagedListRequest : IListRequest
{
    int Skip { get; set; }
    int Take { get; set; }
}

public interface ISortedListRequest : IListRequest
{
    [JsonConverter(typeof(SortDescriptorConverter))]
    ListSortDescriptionCollection Sort { get; set; }
}

public interface IFilteredListRequest : IListRequest
{
    FilterDescriptor Filter { get; set; }
}
