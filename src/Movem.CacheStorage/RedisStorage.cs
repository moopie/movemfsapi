using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.CacheService;

public class RedisStorage(ILogger<RedisStorage> log, IDistributedCache cache) : IStorage
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public async Task<DataModel?> GetAsync(int id)
    {
        var key = id.ToString();
        var cached = await cache.GetStringAsync(key);
        return cached == null ? null : JsonSerializer.Deserialize<DataModel>(cached, _jsonOptions);
    }

    public async Task InsertAsync(DataModel model)
    {
        if (model.Id is not null)
        {
            var exists = await GetAsync(model.Id.Value);
            if (exists is not null)
            {
                return;
            }
        }
        var expiration = TimeSpan.FromMinutes(10);
        var serialized = JsonSerializer.Serialize(model, _jsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(model.Id.ToString()!, serialized, options);
    }

    public async Task UpdateAsync(DataModel model)
    {
        if (model.Id is not null)
        {
            var exists = await GetAsync(model.Id.Value);
            if (exists is not null)
            {
                await cache.RemoveAsync(exists.Id.ToString()!);
            }
        }
        var expiration = TimeSpan.FromMinutes(10);
        var serialized = JsonSerializer.Serialize(model, _jsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(model.Id.ToString()!, serialized, options);
    }
}