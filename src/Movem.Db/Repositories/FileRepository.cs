using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movem.Common.Interfaces;
using Movem.Common.Models;
using Movem.Db.Base;
using Movem.Db.Contexts;
using Movem.Db.Interfaces;
using Movem.Db.Models;

namespace Movem.Db.Repositories;

public class FileRepository(FileStorageContext context, IMapper mapper) : Repository<DataEntity>(context), IFileRepository, IStorage
{
    public async Task<DataModel?> GetAsync(int id)
    {
        var entity = await base.GetAsync(id);
        var model = mapper.Map<DataModel>(entity);
        return model;
    }

    public async Task InsertAsync(DataModel model)
    {
        var entity = mapper.Map<DataEntity>(model);
        await base.AddAsync(entity);
    }

    public Task UpdateAsync(DataModel model)
    {
        var entity = mapper.Map<DataEntity>(model);
        return base.UpdateAsync(entity);
    }
}