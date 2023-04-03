using System.Text.Json.Serialization;

namespace Boilerplate.Common.Data.Querying;

public class PropertyDescriptor
{
    [JsonIgnore]
    public Type ComponentType { get; init; }

    [JsonIgnore]
    public Type PropertyType { get; init; }

    [JsonPropertyName("Field")]
    public string Name { get; init; }
}
