using AutoMapper;
using Movem.Common.Interfaces;
using Movem.Common.Models;
using Movem.Db.Base;
using Movem.Db.Contexts;
using Movem.Db.Interfaces;
using Movem.Db.Models;

namespace Movem.Db.Repositories;

public class FileRepository(
    FileStorageContext context,
    IMapper mapper) : Repository<DataEntity>(context), IFileRepository, IStorage
{
    public async Task<int?> StoreEntityAndGetIdAsync(DataEntity data)
    {
        await AddAsync(data);
        await base.SaveChangesAsync();
        return data.Id;
    }

    public int Priority => 0;
    
    public async Task<DataModel?> GetModelAsync(int id)
    {
        var entity = await GetAsync(id);
        
        if (entity == null) return null;
        
        return mapper.Map<DataModel>(entity);
    }

    public async Task<int?> InsertAsync(DataModel model)
    {
        var entity = mapper.Map<DataEntity>(model);
        await AddAsync(entity);
        await base.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateAsync(DataModel model)
    {
        if (model.Id == null) return;
        var entity = await GetAsync(model.Id.Value);
        
        if (entity == null) return;
        
        entity.FileName = model.FileName;
        entity.Content = model.Content;
        entity.ContentType = model.ContentType;
        entity.Length = model.Length;
        await base.SaveChangesAsync();
    }
}