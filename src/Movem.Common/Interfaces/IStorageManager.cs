using Movem.Common.Models;

namespace Movem.Common.Interfaces;

public interface IStorageManager
{
    Task<DataModel?> GetModelAsync(int id, CancellationToken token);
    Task<int?> InsertModelAsync(DataModel model, CancellationToken token);
    Task<bool> UpdateModelAsync(int id, DataModel model, CancellationToken token);
}