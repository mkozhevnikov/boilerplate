using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boilerplate.Common.Utils;

public static class JsonExtensions
{
    public static string ToJson(this object source, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(source, options);

    public static string ToJson(this object source,
        params JsonConverter[] converters)
    {
        JsonSerializerOptions options = new() {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        foreach (var converter in converters) {
            options.Converters.Add(converter);
        }

        return source.ToJson(options);
    }
}
