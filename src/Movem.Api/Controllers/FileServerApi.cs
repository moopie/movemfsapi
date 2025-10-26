using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movem.Api.DTOs;
using Movem.Common.Interfaces;
using Movem.Common.Models;

namespace Movem.Api.Controllers;

[ApiController]
[Route("api/data")]
public class AppController(
    IStorageManager storageManager,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await storageManager.GetModelAsync(id);

        var file = mapper.Map<FileResponse>(item);
        
        return item is null ? NotFound() : File(file.Data, file.ContentType, file.FileName);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] FileUploadDto data)
    {
        var id = await storageManager.InsertModelAsync(mapper.Map<DataModel>(data));
        return id != null
            ? Ok(id.Value)
            : BadRequest();
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromForm] FileUploadDto file)
    {
        return await storageManager.UpdateModelAsync(id, mapper.Map<DataModel>(file))
            ? Ok()
            : BadRequest();
    }
}
