namespace Movem.Api.DTOs;

public class FileResponse
{
    public byte[] Data { get; set; } = [];
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}