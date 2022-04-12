using System.ComponentModel;
using Boilerplate.Common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boilerplate.Common.Data.Querying;

public class SortDescriptorConverter : JsonConverter<ListSortDescriptionCollection>
{
    public override void WriteJson(JsonWriter writer, ListSortDescriptionCollection? value, JsonSerializer serializer)
    {
        if (value is null) {
            return;
        }

        var jObject = new JArray(
            from sort in value.AsEnumerable<ListSortDescription>()
            select new JObject(
                new JProperty("Field", sort.PropertyDescriptor.Name),
                new JProperty("Dir", SortEnum.FromValue((int)sort.SortDirection).Name)));
        jObject.WriteTo(writer);
    }

    public override ListSortDescriptionCollection? ReadJson(
        JsonReader reader, Type objectType, ListSortDescriptionCollection? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartArray) {
            return hasExistingValue ? existingValue : new ListSortDescriptionCollection();
        }

        reader.Read();
        var collection = new List<ListSortDescription>();
        while (reader.TokenType != JsonToken.EndArray) {
            var obj = JObject.Load(reader);
            collection.Add(new ListSortDescription(
                new JPropertyDescriptor(obj["Field"].Value<string>()),
                SortEnum.FromName(obj["Dir"].Value<string>())
            ));

            reader.Read();
        }

        return new ListSortDescriptionCollection(collection.ToArray());
    }
}
