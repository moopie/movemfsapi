using Microsoft.AspNetCore.Mvc;
using Movem.FileStorage;

namespace Movem.Web.Controllers;

[ApiController]
[Route("api/data")]
public class ProductsController : ControllerBase
{
    private readonly IFileStorage _fileStorage;

    public ProductsController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

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
    public async Task<IActionResult> Put([FromForm] IFormFile file)
    {
        var res = await _fileStorage.StoreFileAsync(file);

        if (res)
        {
            return Ok();
        }

        return BadRequest();
    }
}
