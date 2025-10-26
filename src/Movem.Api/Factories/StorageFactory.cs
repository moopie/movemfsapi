using Movem.CacheService;
using Movem.Common.Enums;
using Movem.Common.Interfaces;

namespace Movem.Api.Factories;

public class StorageFactory(IServiceScopeFactory scopeFactory)
    : IStorageFactory
{
    public IStorage Create(StorageType type)
    {
        var scope = scopeFactory.CreateScope();
        return type switch
        {
            StorageType.FileSystem => scope.ServiceProvider.GetRequiredService<FileStorage.FileStorage>(),
            StorageType.Redis => scope.ServiceProvider.GetRequiredService<RedisStorage>(),
            StorageType.InMemory => scope.ServiceProvider.GetRequiredService<InMemoryStorage>(),
            _ => throw new NotSupportedException($"Storage type '{type}' is not supported")
        };
    }
}