using MongoDB.Driver;

namespace Boilerplate.MongoDB;

public interface ICollectionContext
{
    string Name { get; }

    IMongoDatabase DB { get; }
}