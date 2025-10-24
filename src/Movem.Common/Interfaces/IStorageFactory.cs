using Movem.Common.Enums;

namespace Movem.Common.Interfaces;

public interface IStorageFactory
{
    IStorage Create(StorageType  storageType);
}