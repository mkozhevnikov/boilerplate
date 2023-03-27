using System.Text;
using System.Text.Json;
using Boilerplate.Common.Cache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Boilerplate.Common.UnitTests.Cache;

public class DistributedCacheTests
{
    [Fact]
    public async Task MemoryCache_SetAsJson_DefaultOptions_HitCache()
    {
        var key = "k1";
        var model = new TestModel();

        await cache.SetAsJsonAsync(key, model);

        var cachedValue = await cache.GetAsync(key);
        cachedValue.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public async Task MemoryCache_SetAsJson_ExpiresIn_HitCache()
    {
        var key = "k2";
        var model = new TestModel();

        await cache.SetAsJsonAsync(key, model, TimeSpan.FromDays(1));

        var cachedValue = await cache.GetAsync(key);
        cachedValue.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public async Task MemoryCache_SetAsJson_SlidingExpiration_HitCache()
    {
        var key = "k3";
        var model = new TestModel();

        await cache.SetAsJsonAsync(key, model, new DistributedCacheEntryOptions {
            SlidingExpiration = TimeSpan.FromDays(1)
        });

        var cachedValue = await cache.GetAsync(key);
        cachedValue.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public async Task MemoryCache_SetAsJson_WriteIndented_SerializedIndented()
    {
        var key = "k4";
        var model = new TestModel();

        await cache.SetAsJsonAsync(key, model, jsonSerializerOptions: new JsonSerializerOptions {
            WriteIndented = true
        });

        var cachedValue = await cache.GetAsync(key);
        var jsonSerializedModel = Encoding.UTF8.GetString(cachedValue!);
        jsonSerializedModel.Should().NotBeEmpty().And.MatchRegex("^{(\\s+\"\\w+\\\".+\\n)+}$");
    }

    [Fact]
    public async Task MemoryCache_Exists_HitCache()
    {
        var key = "k5";
        await cache.SetAsync(key, Encoding.UTF8.GetBytes("test value"));

        var sut = await cache.ExistsAsync(key);

        sut.Should().BeTrue();
    }

    [Fact]
    public async Task MemoryCache_Exists_Removed_MissCache()
    {
        var key = "k6";
        await cache.SetAsync(key, Encoding.UTF8.GetBytes("test value"));
        await cache.RemoveAsync(key);

        var sut = await cache.ExistsAsync(key);

        sut.Should().BeFalse();
    }

    [Fact]
    public async Task MemoryCache_GetAsJson_DeserializedToModel()
    {
        var key = "k7";
        var value =
        @"{
          ""IntProp"": 123,
          ""DateTimeProp"": ""2022-04-02""
        }";
        await cache.SetAsync(key, Encoding.UTF8.GetBytes(value));

        var sut = await cache.GetAsJsonAsync<TestModel>(key);

        sut.Should().NotBeNull();
        sut!.IntProp.Should().Be(123);
        sut.DateTimeProp.Should().Be(new DateTime(2022, 4, 2));
    }

    [Fact]
    public async Task MemoryCache_GetAsJson_ReadCaseInsensitive_DeserializedToModel()
    {
        var key = "k8";
        var value =
        @"{
          ""intProp"": 123,
          ""datetimeprop"": ""2022-04-02""
        }";
        await cache.SetAsync(key, Encoding.UTF8.GetBytes(value));

        var sut = await cache.GetAsJsonAsync<TestModel>(key, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        });

        sut.Should().NotBeNull();
        sut!.IntProp.Should().Be(123);
        sut.DateTimeProp.Should().Be(new DateTime(2022, 4, 2));
    }

    [Fact]
    public async Task MemoryCache_GetOrSet_DefaultOptions_CreateValue()
    {
        var key = "k9";
        var model = new TestModel {
            IntProp = 123
        };

        var sut = await cache.GetOrSetAsync(key, model, TimeSpan.FromMinutes(5));

        sut.Should().NotBeNull();
        sut!.IntProp.Should().Be(model.IntProp);
    }

    [Fact]
    public async Task MemoryCache_GetOrSet_UseFactory_CreateValue()
    {
        var key = "k10";
        var model = new TestModel {
            IntProp = 123
        };

        var sut = await cache.GetOrSetAsync(key, () => Task.FromResult(model), TimeSpan.FromMinutes(5));

        sut.Should().NotBeNull();
        sut!.IntProp.Should().Be(model.IntProp);
    }

    [Fact]
    public async Task MemoryCache_GetOrSet_ValueExists_GetValue()
    {
        var key = "k11";
        var model = new TestModel {
            IntProp = 123
        };
        await cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));
        model.DoubleProp = 2d;

        var sut = await cache.GetOrSetAsync(key, model, TimeSpan.FromMinutes(5));

        sut.Should().NotBeNull();
        sut!.IntProp.Should().Be(model.IntProp);
        sut.DoubleProp.Should().NotBe(model.DoubleProp);
    }

    private readonly IDistributedCache cache;

    public DistributedCacheTests()
    {
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();

        var opts = Options.Create(new MemoryDistributedCacheOptions());
        cache = new MemoryDistributedCache(opts);
    }

    private class TestModel
    {
        public int IntProp { get; set; }
        public decimal DecimalProp { get; set; }
        public double DoubleProp { get; set; }
        public string StringProp { get; set; } = null!;
        public DateTime DateTimeProp { get; set; }
    }
}
