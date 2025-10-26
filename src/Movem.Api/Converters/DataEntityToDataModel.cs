using AutoMapper;
using Movem.Common.Models;
using Movem.Db.Models;

namespace Movem.Api.Converters;

public class DataEntityToDataModel : ITypeConverter<DataEntity, DataModel>
{
    public DataModel Convert(DataEntity source, DataModel destination, ResolutionContext context)
    {
        return new DataModel
        {
            Id = source.Id,
            Content = source.Content,
            Length = source.Length,
            ContentType = source.ContentType,
            FileName = source.FileName,
        };
    }
}