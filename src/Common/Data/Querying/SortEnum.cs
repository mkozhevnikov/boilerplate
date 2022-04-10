using System.ComponentModel;
using Ardalis.SmartEnum;

namespace Boilerplate.Common.Data.Querying;

public abstract class SortEnum : SmartEnum<SortEnum>
{
    private const string ascending = "Asc";
    private const string descending = "Desc";

    public static readonly SortEnum Ascending = new AscendingSort();
    public static readonly SortEnum Descending = new DescendingSort();

    public abstract string OrderByMethod { get; }
    public abstract string ThenByMethod { get; }

    public static implicit operator ListSortDirection(SortEnum sortEnum) => (ListSortDirection)sortEnum.Value;

    public static explicit operator SortEnum(ListSortDirection sortDirection) => FromValue((int)sortDirection);

    protected SortEnum(string name, ListSortDirection value) : base(name, (int)value)
    {
    }

    private sealed class AscendingSort : SortEnum
    {
        public override string OrderByMethod => nameof(System.Linq.Queryable.OrderBy);
        public override string ThenByMethod => nameof(System.Linq.Queryable.ThenBy);

        public AscendingSort() : base(ascending, ListSortDirection.Ascending) {}
    }

    private sealed class DescendingSort : SortEnum
    {
        public override string OrderByMethod => nameof(System.Linq.Queryable.OrderByDescending);
        public override string ThenByMethod => nameof(System.Linq.Queryable.ThenByDescending);

        public DescendingSort() : base(descending, ListSortDirection.Descending) {}
    }
}
