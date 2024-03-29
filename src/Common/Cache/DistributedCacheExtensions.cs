using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Boilerplate.Common.Cache;

public static class DistributedCache
{
    private static readonly TimeSpan defaultExpiration = TimeSpan.FromMinutes(1);

    #region GetOrSet

    public static async ValueTask<T> GetOrSetAsync<T>(
        this IDistributedCache cache,
        string key,
        T value,
        TimeSpan? expiresIn,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken token = default) =>
        await cache.GetOrSetAsync(key, () => Task.FromResult(value), expiresIn, jsonSerializerOptions, token);

    public static async Task<T> GetOrSetAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<Task<T>> valueFactory,
        TimeSpan? expiresIn,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken token = default)
    {
        var options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = expiresIn ?? defaultExpiration
        };

        return await cache.GetOrSetAsync(key, valueFactory, options, jsonSerializerOptions, token);
    }

    public static async Task<T> GetOrSetAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<Task<T>> valueFactory,
        DistributedCacheEntryOptions? options = null,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken token = default)
    {
        var result = await cache.GetAsJsonAsync<T>(key, jsonSerializerOptions, token);
        if (result is not null) {
            return result;
        }

        var value = await valueFactory();
        await cache.SetAsJsonAsync(key, value, options, jsonSerializerOptions, token);
        return value;
    }

    #endregion

    #region GetAsJson

    public static async Task<T?> GetAsJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        CancellationToken token = default) =>
        await cache.GetAsJsonAsync<T>(key, null, token);

    public static async Task<T?> GetAsJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        JsonSerializerOptions? jsonSerializerOptions,
        CancellationToken token = default)
    {
        var bytes = await cache.GetAsync(key, token);

        return bytes == null ? default : Deserialize<T>(bytes, jsonSerializerOptions);
    }

    #endregion

    #region SetAsJson

    public static async Task SetAsJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        T? value,
        TimeSpan? expiresIn,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken token = default)
    {
        var options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = expiresIn ?? defaultExpiration
        };

        await cache.SetAsJsonAsync(key, value, options, null, token);
    }

    public static async Task SetAsJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        T? value,
        DistributedCacheEntryOptions? options = null,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken token = default)
    {
        options ??= new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = defaultExpiration
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, jsonSerializerOptions);

        await cache.SetAsync(key, bytes, options, token);
    }

    #endregion

    public static async Task<bool> ExistsAsync(
        this IDistributedCache cache,
        string key,
        CancellationToken token = default) =>
        await cache.GetAsync(key, token) != null;

    private static T? Deserialize<T>(byte[] bytes, JsonSerializerOptions? jsonSerializerOptions)
    {
        var utf8JsonReader = new Utf8JsonReader(bytes);
        return JsonSerializer.Deserialize<T>(ref utf8JsonReader, jsonSerializerOptions);
    }
}
