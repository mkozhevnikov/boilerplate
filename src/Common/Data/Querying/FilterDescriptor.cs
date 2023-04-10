using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;

namespace Boilerplate.Common.Data.Querying;

/// <summary>
/// A basic filter expression. Usually is a part of <see cref="CompositeFilterDescriptor"/>
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(CompositeFilterDescriptor), typeDiscriminator: "withLogic")]
public class FilterDescriptor
{
    /// <summary>
    /// The data item field to which the filter operator is applied.
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// The filter operator (comparison).
    /// </summary>
    [JsonConverter(typeof(SmartEnumNameConverter<Operator, int>))]
    public Operator? Operator { get; set; }

    /// <summary>
    /// The value to which the field is compared. Has to be of the same type as the field.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Determines if the string comparison is case-insensitive.
    /// </summary>
    public bool? IgnoreCase { get; set; }
}
