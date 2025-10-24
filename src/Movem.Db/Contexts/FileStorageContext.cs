using Microsoft.EntityFrameworkCore;
using Movem.Db.Models;

namespace Movem.Db.Contexts;

public sealed class FileStorageContext(
    DbContextOptions<FileStorageContext> options) : DbContext(options)
{
    public DbSet<DataEntity> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<DataEntity>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }
}