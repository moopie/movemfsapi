using Microsoft.EntityFrameworkCore;
using Movem.Db.Contexts;
using Movem.Db.Interfaces;
using Movem.Db.Models;

namespace Movem.Db.Repositories;

public class FileRepository(FileStorageContext context) : IRepository<FileData>
{
    public async Task<IEnumerable<FileData>> GetAllAsync()
    {
        return await context.Files.ToArrayAsync();
    }

    public async Task<FileData?> GetByIdAsync(int id)
    {
        return await context.Files.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<int> AddAsync(FileData entity)
    {
        context.Files.Add(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Update(FileData entity)
    {
        context.Files.Update(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(FileData entity)
    {
        context.Files.Remove(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}