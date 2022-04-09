using Ardalis.SmartEnum.JsonNet;
using JsonSubTypes;
using Newtonsoft.Json;

namespace Boilerplate.Common.Data.Querying;

/// <summary>
/// A basic filter expression. Usually is a part of <see cref="CompositeFilterDescriptor"/>
/// </summary>
[JsonConverter(typeof(JsonSubtypes))]
[JsonSubtypes.KnownSubTypeWithPropertyAttribute(
    typeof(CompositeFilterDescriptor), nameof(CompositeFilterDescriptor.Logic))]
[JsonSubtypes.KnownSubTypeWithPropertyAttribute(typeof(FilterDescriptor), nameof(Operator))]
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class FilterDescriptor
{
    /// <summary>
    /// The data item field to which the filter operator is applied.
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// The filter operator (comparison).
    /// </summary>
    [JsonConverter(typeof(SmartEnumNameConverter<OperatorEnum, int>))]
    public OperatorEnum Operator { get; set; }

    /// <summary>
    /// The value to which the field is compared. Has to be of the same type as the field.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Determines if the string comparison is case-insensitive.
    /// </summary>
    public bool? IgnoreCase { get; set; }
}
