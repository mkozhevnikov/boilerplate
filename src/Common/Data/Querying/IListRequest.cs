using Boilerplate.Common.Utils;

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
    ICollection<SortingDescriptor> Sort { get; set; }
}

public interface IFilteredListRequest : IListRequest
{
    FilterDescriptor Filter { get; set; }
}

public static class ListRequestExtensions
{
    public static ISpec<T> ToSpec<T>(this IListRequest request)
    {
        var spec = request is IFilteredListRequest filteredRequest
            ? new Spec<T>(filteredRequest.Filter.ToExpression<T>())
            : Specs.True<T>();

        if (request is IPagedListRequest pagedRequest) {
            spec.Skip = pagedRequest.Skip;
            spec.Take = pagedRequest.Take;
        }

        if (request is ISortedListRequest sortedRequest) {
            return new SortingSpec<T>(spec) {
                Sorting = sortedRequest.Sort
            };
        }

        return spec;
    }
}
