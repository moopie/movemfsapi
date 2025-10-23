namespace Movem.Common.Interfaces;

public interface IStorageProvider<T>
{
    Task<T?> GetAsync(int id);
    Task<bool> InsertAsync(T item);
    Task<bool> UpdateAsync(T item);
}