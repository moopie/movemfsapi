using Movem.Common.Models;

namespace Movem.Common.Interfaces;

public interface IStorage
{
    Task<DataModel?> GetAsync(int id);
    Task InsertAsync(DataModel model);
    Task UpdateAsync(DataModel model);
}