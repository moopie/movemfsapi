using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Movem.FileStorage;

public class FileStorage(IOptions<FileStorageOptions> options) : IFileStorage
{
    private readonly FileStorageOptions _options = options.Value;

    public async Task<bool> StoreFileAsync(IFormFile file)
    {
        if (file.Length > _options.MaxFileSize)
            return false;

        var path = Path.Combine(_options.RootPath, file.FileName);
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return true;
    }
}