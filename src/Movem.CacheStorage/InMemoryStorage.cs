using System.Reflection.Metadata.Ecma335;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.CacheService;

public class InMemoryStorage : IStorage
{
    private readonly Dictionary<int, (DataModel, DateTime, TimeSpan)> _cache = new();
    public async Task<DataModel?> GetAsync(int id)
    {
        var (file, start, expirationTime) = _cache.GetValueOrDefault(id);

        if (DateTime.UtcNow - start > expirationTime)
        {
            return null;
        }

        return file;
    }

    public async Task InsertAsync(DataModel model)
    {
        if (!model.Id.HasValue) return;
        var m = await GetAsync(model.Id.Value);
        if (m is not null) return;
        _cache.Add(model.Id.Value, (model, DateTime.UtcNow, TimeSpan.FromMinutes(10)));
    }

    public Task UpdateAsync(DataModel model)
    {
        if (!model.Id.HasValue) return Task.CompletedTask;
        _cache.Remove(model.Id.Value);
        _cache.Add(model.Id.Value, (model, DateTime.UtcNow, TimeSpan.FromMinutes(10)));
        return Task.CompletedTask;
    }
}