using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Boilerplate.MongoDB;

public class StringObjectIdGenerator : IIdGenerator
{
    public object GenerateId(object container, object document) => ObjectId.GenerateNewId(DateTime.UtcNow).ToString();

    public bool IsEmpty(object id) => string.IsNullOrEmpty((string)id);
}