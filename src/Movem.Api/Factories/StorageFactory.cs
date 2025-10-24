using Movem.CacheService;
using Movem.Common.Enums;
using Movem.Common.Interfaces;

namespace Movem.Api.Factories;

public class StorageFactory : IStorageFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScopeFactory _scopeFactory;

    public StorageFactory(IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory)
    {
        _serviceProvider = serviceProvider;
        _scopeFactory = scopeFactory;
    }

    public IStorage Create(StorageType type)
    {
        var scope = _scopeFactory.CreateScope();
        return type switch
        {
            StorageType.FileSystem => scope.ServiceProvider.GetRequiredService<FileStorage.FileStorage>(),
            StorageType.Redis => scope.ServiceProvider.GetRequiredService<RedisStorage>(),
            StorageType.InMemory => scope.ServiceProvider.GetRequiredService<InMemoryStorage>(),
            _ => throw new NotSupportedException($"Storage type '{type}' is not supported")
        };
    }
}