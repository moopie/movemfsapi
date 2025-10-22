using System.ComponentModel.DataAnnotations.Schema;

namespace Movem.Db.Models;

public sealed class FileData
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string Data { get; set; }
}