namespace Boilerplate.Common.Data.Querying;

/// <summary>
/// A complex filter expression.
/// </summary>
public class CompositeFilterDescriptor : FilterDescriptor
{
    /// <summary>
    /// The logical operation to use when the `filter.filters` option is set.
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// The nested filter expressions. Supports the same options as <see cref="FilterDescriptor"/>.
    /// You can nest filters indefinitely.
    /// </summary>
    public IEnumerable<FilterDescriptor> Filters { get; set; }
}
