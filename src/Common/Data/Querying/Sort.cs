using System.ComponentModel;
using Ardalis.SmartEnum;

namespace Boilerplate.Common.Data.Querying;

public abstract class Sort : SmartEnum<Sort>
{
    private const string ascending = "Asc";
    private const string descending = "Desc";

    public static readonly Sort Ascending = new AscendingSort();
    public static readonly Sort Descending = new DescendingSort();

    public abstract string OrderByMethod { get; }
    public abstract string ThenByMethod { get; }

    public static implicit operator ListSortDirection(Sort sort) => (ListSortDirection)sort.Value;

    public static explicit operator Sort(ListSortDirection sortDirection) => FromValue((int)sortDirection);

    protected Sort(string name, ListSortDirection value) : base(name, (int)value)
    {
    }

    private sealed class AscendingSort : Sort
    {
        public override string OrderByMethod => nameof(System.Linq.Queryable.OrderBy);
        public override string ThenByMethod => nameof(System.Linq.Queryable.ThenBy);

        public AscendingSort() : base(ascending, ListSortDirection.Ascending) {}
    }

    private sealed class DescendingSort : Sort
    {
        public override string OrderByMethod => nameof(System.Linq.Queryable.OrderByDescending);
        public override string ThenByMethod => nameof(System.Linq.Queryable.ThenByDescending);

        public DescendingSort() : base(descending, ListSortDirection.Descending) {}
    }
}
