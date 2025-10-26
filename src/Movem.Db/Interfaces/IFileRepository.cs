using Movem.Db.Models;

namespace Movem.Db.Interfaces
{
    public interface IFileRepository : IRepository<DataEntity>
    {
        public Task<int?> StoreEntityAndGetIdAsync(DataEntity data);
    }
}