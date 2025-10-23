using AutoMapper;
using Movem.Api.DTOs;
using Movem.Common.Interfaces;
using Movem.Db.Interfaces;
using Movem.Db.Models;
using Movem.Db.Repositories;

namespace Movem.Api.Services;

public class StorageFactoryService(IMapper mapper, IRepository<FileData> repository) : IStorageFactoryService<DataDto>
{
    public async Task<DataDto?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> InsertAsync(DataDto item)
    {
        var fileData = mapper.Map<FileData>(item);
        await repository.AddAsync(fileData);
        return true;
    }

    public async Task<bool> UpdateAsync(DataDto item)
    {
        throw new NotImplementedException();
    }
}