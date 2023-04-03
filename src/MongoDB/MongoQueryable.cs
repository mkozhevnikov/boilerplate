using Boilerplate.Common.Data;
using Boilerplate.Common.Data.Querying;
using MongoDB.Driver.Linq;

namespace Boilerplate.MongoDB;

public static class MongoQueryable
{
    public static IMongoQueryable<T> Sort<T>(this IMongoQueryable<T> queryable, ISpec<T> spec) =>
        (IMongoQueryable<T>)(spec is ISortingSpec<T> sortedSpec ? queryable.Sort(sortedSpec.Sorting) : queryable);
}
