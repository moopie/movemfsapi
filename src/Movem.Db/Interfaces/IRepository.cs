namespace Movem.Db.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<int> AddAsync(T entity);
    Task<int> Update(T entity);
    Task<int> Delete(T entity);
    Task<int> SaveChangesAsync();
}