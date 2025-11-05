using System.Text.Json;
using Microsoft.Extensions.Logging;
using Movem.Common.Enums;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.FileStorage;

public class FileStorage(ILogger<FileStorage> log) : IStorage
{
    public int Priority => 100;
    public StorageType StorageType => StorageType.FileSystem;

    // TODO: Move to configuration file
    //private readonly string _rootPath = $"{Path.Combine(Path.GetTempPath(), "storage")}";
    private readonly string _rootPath = $"{Path.Combine(AppContext.BaseDirectory, "storage")}";
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task<DataModel?> GetModelAsync(int id, CancellationToken token)
    {
        var file = Directory.GetFiles(_rootPath, $"{id}_*.json").SingleOrDefault();
        if (file is null) return null;
        
        var ts = long.Parse(Path.GetFileNameWithoutExtension(file).Split('_')[1]);
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (now > ts)
        {
            log.LogInformation($"Removing file {file} because it expires at {ts} and now it's {now}.");
            File.Delete(file);
            return null;
        }
        
        try
        {
            await using var stream = File.OpenRead(file);
            return await JsonSerializer.DeserializeAsync<DataModel>(stream, _jsonSerializerOptions);
        }
        catch (Exception e)
        {
            log.LogError(e.Message);
            return null;
        }
    }

    public async Task<int?> InsertAsync(DataModel file, CancellationToken token)
    {
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
        
        if (!file.Id.HasValue) return null;
        
        var exists = await GetModelAsync(file.Id.Value, token);
        if (exists is not null) return null;
        
        var ts = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeMilliseconds();
        var name = $"{file.Id}_{ts}.json";
        var path = Path.Combine(_rootPath, name);
        try
        {
            await using var stream = File.Create(path);
            await JsonSerializer.SerializeAsync(stream, file, _jsonSerializerOptions);
            return file.Id.Value;
        }
        catch (Exception e)
        {
            log.LogError(e, e.Message);
            throw;
        }
    }

    public async Task UpdateAsync(DataModel model, CancellationToken token)
    {
        var file = Directory.GetFiles(_rootPath, $"{model.Id}_*.json").SingleOrDefault();
        if (file is not null)
        {
            File.Delete(file);
        }
        
        await InsertAsync(model, token);
        
    }
}