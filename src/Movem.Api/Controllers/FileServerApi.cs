using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movem.Api.DTOs;
using Movem.Common.Interfaces;
using Movem.FileStorage;

namespace Movem.Api.Controllers;

[ApiController]
[Route("api/data")]
public class AppController(
    ILogger<AppController> log,
    IStorageFactoryService<DataDto> storageService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok();
    }

    [HttpPost("{data}")]
    public async Task<IActionResult> Post(object data)
    {
        return Ok();
    }

    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put([FromForm] FileUploadDto file)
    {
        var dto = mapper.Map<DataDto>(file);
        var res = await storageService.InsertAsync(dto);

        if (res)
        {
            return Ok();
        }

        return BadRequest();
    }
}
