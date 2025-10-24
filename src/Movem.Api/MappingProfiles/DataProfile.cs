using AutoMapper;
using Movem.Api.Converters;
using Movem.Api.DTOs;
using Movem.Common.Models;
using Movem.Db.Models;

namespace Movem.Api.MappingProfiles;

public class DataProfile : Profile
{
    public DataProfile()
    {
        CreateMap<FileUploadDto, DataModel>()
            .ConvertUsing<FileUploadDtoToDataModel>();
        CreateMap<FileUploadDto, DataEntity>()
            .ConvertUsing<FileUploadDtoToDataEntity>();
        CreateMap<DataEntity, DataModel>()
            .ConvertUsing<DataEntityToDataModel>();
        CreateMap<DataModel, FileResponse>()
            .ConvertUsing<DataModelToFile>();
    }
}