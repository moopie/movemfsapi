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

        public async Task<T?> GetAsync(int id, CancellationToken token)
        {
            return await _dbSet.FindAsync(id, token);
        }

        public async Task AddAsync(T entity, CancellationToken token)
        {
            await _dbSet.AddAsync(entity, token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync(token);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken token)
        {
            await _context.SaveChangesAsync(token);
        }
    }
}