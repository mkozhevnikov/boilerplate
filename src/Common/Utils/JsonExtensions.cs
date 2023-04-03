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

    internal static void EnsureJsonValueKindIsArray(JsonElement jsonElement)
    {
        const string errorMessage = "Can't iterate non array element";
        if (jsonElement.ValueKind != JsonValueKind.Array) {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static IEnumerable<string?> AsStringArray(this JsonElement jsonElement)
    {
        EnsureJsonValueKindIsArray(jsonElement);
        foreach (JsonElement element in jsonElement.EnumerateArray()) {
            yield return element.GetString();
        }
    }

    public static IEnumerable<int> AsIntArray(this JsonElement jsonElement)
    {
        EnsureJsonValueKindIsArray(jsonElement);
        foreach (JsonElement element in jsonElement.EnumerateArray()) {
            yield return element.GetInt32();
        }
    }
}
