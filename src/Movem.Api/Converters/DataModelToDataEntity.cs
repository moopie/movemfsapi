using AutoMapper;
using Movem.Common.Models;
using Movem.Db.Models;

namespace Movem.Api.Converters;

public class DataModelToDataEntity : ITypeConverter<DataModel, DataEntity>
{
    public DataEntity Convert(DataModel source, DataEntity destination, ResolutionContext context)
    {
        return new DataEntity
        {
            Id = source.Id ?? 0,
            FileName = source.FileName,
            Length = source.Length,
            ContentType = source.ContentType,
            Content = source.Content
        };
    }
}