using System.ComponentModel;
using Ardalis.SmartEnum;

namespace Boilerplate.Common.Data.Querying;

public class SortEnum : SmartEnum<SortEnum>
{
    private const string ascending = "Asc";
    public static readonly SortEnum Ascending = new(ascending, ListSortDirection.Ascending);

    private const string descending = "Desc";
    public static readonly SortEnum Descending = new(descending, ListSortDirection.Descending);

    public static implicit operator ListSortDirection(SortEnum sortEnum) => (ListSortDirection)sortEnum.Value;

    public SortEnum(string name, ListSortDirection value) : base(name, (int)value)
    {
    }
}
