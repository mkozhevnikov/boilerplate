using System;
using Boilerplate.MongoDB.Sample.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Bson.Serialization;

namespace Boilerplate.MongoDB.Sample;

public partial class Startup
{
    private IMongoClient CreateClient(IServiceProvider provider)
    {
        BsonClassMap.RegisterClassMap<Customer>(cm => {
            cm.MapIdProperty(e => e.Id).SetIdGenerator(new StringObjectIdGenerator());
            cm.AutoMap();
        });

        var mongoClientSettings = new MongoClientSettings
        {
            Server = MongoServerAddress.Parse("localhost:27017"),
            Scheme = ConnectionStringScheme.MongoDB,
            UseTls = false,
            RetryWrites = true,
            WriteConcern = WriteConcern.WMajority,
        };

        return new MongoClient(mongoClientSettings);
    }

    public class CustomerCollectionContext : ICollectionContext
    {
        public string Name => "customers";
        public IMongoDatabase DB { get; }

        public CustomerCollectionContext(IMongoDatabase database) => DB = database;
    }
}