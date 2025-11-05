using Movem.Common.Enums;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.CacheService;

public class InMemoryStorage : IStorage
{
    public int Priority => 10;
    public StorageType StorageType => StorageType.InMemory;
    
    private static readonly Dictionary<int, (DataModel, DateTime, TimeSpan)> Cache = new();
    public Task<DataModel?> GetModelAsync(int id, CancellationToken token)
    {
        if (!Cache.TryGetValue(id, out var model))
        {
            return Task.FromResult<DataModel?>(null);
        }
        
        var (file, start, expirationTime) = model;

        if (DateTime.UtcNow - start > expirationTime)
        {
            return Task.FromResult<DataModel?>(null);
        }

        return Task.FromResult(file)!;
    }

    public async Task<int?> InsertAsync(DataModel model, CancellationToken token)
    {
        if (!model.Id.HasValue) return null;
        var m = await GetModelAsync(model.Id.Value, token);
        if (m is not null) return null;
        Cache.Add(model.Id.Value, (model, DateTime.UtcNow, TimeSpan.FromMinutes(10)));
        return model.Id.Value;
    }

    public Task UpdateAsync(DataModel model, CancellationToken token)
    {
        if (!model.Id.HasValue) return Task.CompletedTask;
        Cache.Remove(model.Id.Value);
        Cache.Add(model.Id.Value, (model, DateTime.UtcNow, TimeSpan.FromMinutes(10)));
        return Task.CompletedTask;
    }
}