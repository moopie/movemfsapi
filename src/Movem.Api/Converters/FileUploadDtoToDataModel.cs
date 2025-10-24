using AutoMapper;
using Movem.Api.DTOs;
using Movem.Common.Models;

namespace Movem.Api.Converters;

public class FileUploadDtoToDataModel : ITypeConverter<FileUploadDto, DataModel>
{
    public DataModel Convert(FileUploadDto source, DataModel destination, ResolutionContext context)
    {
        using var ms = new MemoryStream();
        source.File.CopyTo(ms);
        var bytes = ms.ToArray();
        var base64 = System.Convert.ToBase64String(bytes);

        return new DataModel
        {
            Content = base64,
            ContentType = source.File.ContentType,
            Length = source.File.Length,
            FileName = source.File.FileName
        };
    }
}