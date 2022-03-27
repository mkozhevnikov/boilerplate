namespace Boilerplate.MongoDB;

using global::MongoDB.Driver;

public interface ICollectionContext
{
    string Name { get; }

    IMongoDatabase DB { get; }
}
