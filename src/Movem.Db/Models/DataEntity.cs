using System.ComponentModel.DataAnnotations.Schema;

namespace Movem.Db.Models;

public sealed class DataEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FileName { get; set; }
    public long Length { get; set; }
    public string ContentType { get; set; }
    public string Content { get; set; }
}