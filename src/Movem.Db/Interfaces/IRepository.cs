namespace Movem.Db.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task SaveChangesAsync();
    }
}
