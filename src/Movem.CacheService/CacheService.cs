using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Movem.CacheService;

public class CacheService(ILogger<CacheService> log, IDistributedCache cache)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public async Task<T?> GetAsync<T>(int id)
    {
        var key = id.ToString();
        var cached = await cache.GetStringAsync(key);
        return cached == null ? default : JsonSerializer.Deserialize<T>(cached, _jsonOptions);
    }

    public async Task InsertAsync<T>(int id, T value, TimeSpan expiration)
    {
        var key = id.ToString();
        var serialized = JsonSerializer.Serialize(value, _jsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(key, serialized, options);
    }
}