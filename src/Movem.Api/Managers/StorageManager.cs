using AutoMapper;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.Api.Managers;

public class StorageManager(
    ILogger<StorageManager> log,
    IMapper mapper,
    [FromKeyedServices("primary")]
    IStorage primaryStorage,
    [FromKeyedServices("secondary")]
    IEnumerable<IStorage> secondaryStorages) : IStorageManager
{
    public async Task<DataModel?> GetModelAsync(int id)
    {
        var notHit = new List<IStorage>();

        DataModel? model = null;
        foreach (var storage in secondaryStorages.OrderBy(s => s.Priority))
        {
            if (model != null)
            {
                continue;
            }
            
            var item = await storage.GetModelAsync(id);

            if (item != null)
            {
                model = item;
            }
            else
            {
                notHit.Add(storage);
            }
        }

        // No data was found anywhere, get it from the db
        if (model is null)
        {
            var item = await primaryStorage.GetModelAsync(id);
            if (item is null)
            {
                return null;
            }
            
            model = item;
        }
        
        // Go over storages that don't have the data and insert it there.
        if (notHit.Count > 0)
        {
            foreach (var storage in notHit)
            {
                try
                {
                    await storage.InsertAsync(model);
                }
                catch (Exception e)
                {
                    log.LogError(e.Message);
                    // Can be optional, can throw
                    // Not sure what to do here
                    //throw;
                }
            }
        }

        return model;
    }

    public async Task<int?> InsertModelAsync(DataModel model)
    {
        if (model.Id is not null)
        {
            return null;
        }
        
        var id = await primaryStorage.InsertAsync(model);
        if (id is null) return null;
        
        model.Id = id;
        
        foreach (var storage in secondaryStorages)
        {
            try
            {
                await storage.InsertAsync(model);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                // See line 65
                //throw;
            }
        }

        return id;
    }

    public async Task<bool> UpdateModelAsync(int id, DataModel model)
    {
        model.Id = id;
        
        await primaryStorage.UpdateAsync(model);

        foreach (var storage in secondaryStorages)
        {
            try
            {
                await storage.UpdateAsync(model);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                // See line 65
                //throw;
            }
        }
        
        return true;
    }
}