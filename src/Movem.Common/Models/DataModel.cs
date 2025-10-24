namespace Movem.Common.Models;

public record DataModel
{
    public int? Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public long Length { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public required string FileName { get; set; }
}