using AutoMapper;
using Movem.Api.DTOs;
using Movem.Common.Models;

namespace Movem.Api.Converters;

public class DataModelToFile : ITypeConverter<DataModel, FileResponse>
{
    public FileResponse Convert(DataModel source, FileResponse destination, ResolutionContext context)
    {
        return new FileResponse()
        {
            ContentType = source.ContentType,
            FileName = source.FileName,
            Data = System.Convert.FromBase64String(source.Content)
        };
    }
}