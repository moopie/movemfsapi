using Microsoft.AspNetCore.Http;

namespace Movem.FileStorage;

public interface IFileStorage
{
    Task<bool> StoreFileAsync(IFormFile file);
}