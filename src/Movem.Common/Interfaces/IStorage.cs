using Movem.Common.Models;

namespace Movem.Common.Interfaces;

public interface IStorage
{
    public int Priority { get; }
    Task<DataModel?> GetModelAsync(int id, CancellationToken token);
    Task<int?> InsertAsync(DataModel model, CancellationToken token);
    Task UpdateAsync(DataModel model, CancellationToken token);
}