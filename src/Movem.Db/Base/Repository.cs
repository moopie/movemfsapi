using Microsoft.EntityFrameworkCore;
using Movem.Db.Contexts;
using Movem.Db.Interfaces;

namespace Movem.Db.Base
{
    public class Repository<T>(FileStorageContext context) : IRepository<T>
        where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}