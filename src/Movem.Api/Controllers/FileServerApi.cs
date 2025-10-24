using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movem.Api.DTOs;
using Movem.Common.Enums;
using Movem.Common.Interfaces;
using Movem.Common.Models;
using Movem.Db.Interfaces;
using Movem.Db.Models;
using Movem.FileStorage;

namespace Movem.Api.Controllers;

[ApiController]
[Route("api/data")]
public class AppController(
    ILogger<AppController> log,
    IStorageFactory storage,
    IFileRepository repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var cache = storage.Create(StorageType.InMemory);
        var cacheItem = await cache.GetAsync(id);
        if (cacheItem is not null)
        {
            var fr = mapper.Map<FileResponse>(cacheItem);
            return File(fr.Data, fr.ContentType, fr.FileName);
        }

        var fs = storage.Create(StorageType.FileSystem);
        var fsItem = await fs.GetAsync(id);

        if (fsItem is not null)
        {
            await cache.InsertAsync(fsItem);
            var fr = mapper.Map<FileResponse>(fsItem);
            return File(fr.Data, fr.ContentType, fr.FileName);
        }
        
        var dbItem = await repository.GetAsync(id);

        if (dbItem is not null)
        {
            var model = mapper.Map<DataModel>(dbItem);
            await fs.InsertAsync(model);
            await cache.InsertAsync(model);
            var fr = mapper.Map<FileResponse>(model);
            return File(fr.Data, fr.ContentType, fr.FileName);
        }

        return NotFound();
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] FileUploadDto data)
    {
        var entity = mapper.Map<DataEntity>(data);
        await repository.AddAsync(entity);
        await repository.SaveChangesAsync();
        var model = mapper.Map<DataModel>(entity);
        
        var fs = storage.Create(StorageType.FileSystem);
        await fs.InsertAsync(model);
        
        var cache = storage.Create(StorageType.InMemory);
        await cache.InsertAsync(model);

        return Ok(entity.Id);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromForm] FileUploadDto file)
    {
        var entity = mapper.Map<DataEntity>(file);
        entity.Id = id;
        await repository.UpdateAsync(entity);
        
        var model = mapper.Map<DataModel>(entity);
        
        var fs = storage.Create(StorageType.FileSystem);
        await fs.UpdateAsync(model);
        
        var cache = storage.Create(StorageType.InMemory);
        await cache.UpdateAsync(model);

        return Ok();
    }
}
