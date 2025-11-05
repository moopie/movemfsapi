using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movem.Api.DTOs;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.Api.Controllers;

[ApiController]
[Route("api/data")]
public class AppController(
    ILogger<AppController> log,
    IStorageManager storageManager,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(CancellationToken token, int id)
    {
        try
        {
            var item = await storageManager.GetModelAsync(id, token);

            if (item == null) return NotFound();

            var file = mapper.Map<FileResponse>(item);
        
            return File(file.Data, file.ContentType, file.FileName);
        }
        catch (Exception e)
        {
            log.LogError(e, e.Message);
            return Problem();
        }
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post(CancellationToken token, [FromForm] FileUploadDto data)
    {
        try
        {
            var id = await storageManager.InsertModelAsync(mapper.Map<DataModel>(data), token);
            return id != null
                ? Ok(id.Value)
                : BadRequest();
        }
        catch (Exception e)
        {
            log.LogError(e, e.Message);
            return Problem();
        }
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put(CancellationToken token, [FromRoute] int id, [FromForm] FileUploadDto file)
    {
        try
        {
            return await storageManager.UpdateModelAsync(id, mapper.Map<DataModel>(file), token)
                ? Ok()
                : BadRequest();
        }
        catch (Exception e)
        {
            log.LogError(e, e.Message);
            return Problem();
        }
    }
}
