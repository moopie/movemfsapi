using AutoMapper;
using Movem.Api.Converters;
using Movem.Api.DTOs;

namespace Movem.Api.MappingProfiles;

public class DataProfile : Profile
{
    public DataProfile()
    {
        CreateMap<FileUploadDto, DataDto>()
            .ConvertUsing<FileUploadDtoToDataDto>();
    }
}