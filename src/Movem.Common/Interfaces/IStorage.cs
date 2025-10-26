using Movem.Common.Models;

namespace Movem.Common.Interfaces;

public interface IStorage
{
    public int Priority { get; }
    Task<DataModel?> GetModelAsync(int id);
    Task<int?> InsertAsync(DataModel model);
    Task UpdateAsync(DataModel model);
}