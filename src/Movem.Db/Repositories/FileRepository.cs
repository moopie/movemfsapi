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
    public int Priority => 0;
    
    public async Task<DataModel?> GetModelAsync(int id, CancellationToken token)
    {
        var entity = await GetAsync(id, token);
        
        if (entity == null) return null;
        
        return mapper.Map<DataModel>(entity);
    }

    public async Task<int?> InsertAsync(DataModel model, CancellationToken token)
    {
        var entity = mapper.Map<DataEntity>(model);
        await AddAsync(entity, token);
        await base.SaveChangesAsync(token);

        return entity.Id;
    }

    public async Task UpdateAsync(DataModel model, CancellationToken token)
    {
        if (model.Id == null) return;
        var entity = await GetAsync(model.Id.Value, token);
        
        if (entity == null) return;
        
        entity.FileName = model.FileName;
        entity.Content = model.Content;
        entity.ContentType = model.ContentType;
        entity.Length = model.Length;
        await base.SaveChangesAsync(token);
    }
}