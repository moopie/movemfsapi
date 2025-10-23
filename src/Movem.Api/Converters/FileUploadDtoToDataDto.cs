using AutoMapper;
using Movem.Api.DTOs;

namespace Movem.Api.Converters;

public class FileUploadDtoToDataDto : ITypeConverter<FileUploadDto, DataDto>
{
    public DataDto Convert(FileUploadDto source, DataDto destination, ResolutionContext context)
    {
        using var ms = new MemoryStream();
        source.File.CopyTo(ms);
        var bytes = ms.ToArray();
        var base64 = System.Convert.ToBase64String(bytes);

        return new DataDto
        {
            Content = base64
        };
    }
}