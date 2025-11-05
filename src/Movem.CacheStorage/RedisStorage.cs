using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Movem.Common.Enums;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.CacheService;

public class RedisStorage(ILogger<RedisStorage> log, IDistributedCache cache) : IStorage
{
    public int Priority => 10;
    public StorageType StorageType => StorageType.Redis;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public async Task<DataModel?> GetModelAsync(int id, CancellationToken token)
    {
        var key = id.ToString();
        var cached = await cache.GetStringAsync(key, token);
        return cached == null ? null : JsonSerializer.Deserialize<DataModel>(cached, _jsonOptions);
    }

    public async Task<int?> InsertAsync(DataModel model, CancellationToken token)
    {
        if (model.Id is null) return null;
        var exists = await GetModelAsync(model.Id.Value, token);
        if (exists is not null)
        {
            return null;
        }
        var expiration = TimeSpan.FromMinutes(10);
        var serialized = JsonSerializer.Serialize(model, _jsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(model.Id.ToString()!, serialized, options, token);
        return model.Id!.Value;
    }

    public async Task UpdateAsync(DataModel model, CancellationToken token)
    {
        if (model.Id is null) return;
        
        var exists = await GetModelAsync(model.Id.Value, token);
        if (exists is not null)
        {
            await cache.RemoveAsync(exists.Id.ToString()!);
        }
        var expiration = TimeSpan.FromMinutes(10);
        var serialized = JsonSerializer.Serialize(model, _jsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(model.Id.ToString()!, serialized, options, token);
    }
}