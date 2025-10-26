using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movem.Db.Base;
using Movem.Db.Contexts;
using Movem.Db.Interfaces;
using Movem.Db.Repositories;

namespace Movem.Db.Exrensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddMovemDb(this IServiceCollection collection, IConfiguration configuration)
    {
        // Ideally I'd just build the connection string from the .env file
        // but I'm not sure if the service will be run with the environment variables in mind
        collection.AddDbContext<DbContext, FileStorageContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        collection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        collection.AddScoped<IFileRepository, FileRepository>();
        return collection;
    }
}