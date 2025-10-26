using Movem.Common.Models;

namespace Movem.Common.Interfaces;

public interface IStorageManager
{
    Task<DataModel?> GetModelAsync(int id);
    Task<int?> InsertModelAsync(DataModel model);
    Task<bool> UpdateModelAsync(int id, DataModel model);
}