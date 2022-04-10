using Newtonsoft.Json;

namespace Boilerplate.Common.Utils;

public static class JsonExtensions
{
    public static string ToJson(this object source,
        Formatting formatting = Formatting.None,
        JsonSerializerSettings? serializerSettings = null)
    {
        return JsonConvert.SerializeObject(source, formatting, serializerSettings);
    }

    public static string ToJson(this object source,
        Formatting formatting = Formatting.None,
        params JsonConverter[] converters)
    {
        return JsonConvert.SerializeObject(source, formatting, converters);
    }
}
