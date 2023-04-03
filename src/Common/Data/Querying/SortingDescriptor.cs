using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;

namespace Boilerplate.Common.Data.Querying;

public class SortingDescriptor
{
    [JsonPropertyName("Field")]
    public string Field {
        get => Property.Name;
        set => Property = new PropertyDescriptor { Name = value };
    }

    [JsonIgnore]
    public PropertyDescriptor Property { get; set; }

    [JsonPropertyName("Dir")]
    [JsonConverter(typeof(SmartEnumNameConverter<Sort, int>))]
    public Sort Direction { get; init; }
}
