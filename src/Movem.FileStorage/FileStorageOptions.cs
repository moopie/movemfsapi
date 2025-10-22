namespace Movem.FileStorage;

public record FileStorageOptions
{
    public string RootPath { get; set; } = $"{Path.Combine(Path.GetTempPath(), "storage")}";
    public long MaxFileSize { get; set; } = 5 * 1024 * 1024;
}