namespace Boilerplate.MongoDB;

using global::MongoDB.Bson;
using global::MongoDB.Bson.Serialization;

public class StringObjectIdGenerator : IIdGenerator
{
    public object GenerateId(object container, object document) => ObjectId.GenerateNewId(DateTime.UtcNow).ToString();

    public bool IsEmpty(object id) => string.IsNullOrEmpty((string)id);
}