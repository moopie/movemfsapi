using AutoMapper;
using Movem.Api.DTOs;
using Movem.Db.Models;

namespace Movem.Api.Converters;

public class FileUploadDtoToDataEntity : ITypeConverter<FileUploadDto, DataEntity>
{
    public DataEntity Convert(FileUploadDto source, DataEntity destination, ResolutionContext context)
    {
        using var ms = new MemoryStream();
        source.File.CopyTo(ms);
        var bytes = ms.ToArray();
        var base64 = System.Convert.ToBase64String(bytes);

        return new DataEntity
        {
            ContentType = source.File.ContentType,
            Length = source.File.Length,
            Content = base64,
            FileName = source.File.FileName
        };
    }
}