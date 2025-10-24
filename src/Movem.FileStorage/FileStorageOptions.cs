namespace Movem.FileStorage;

public record FileStorageOptions
{
    public string RootPath { get; set; } = $"{Path.Combine(Path.GetTempPath(), "storage")}";
}