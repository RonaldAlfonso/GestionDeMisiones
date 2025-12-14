using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.IService;
using GestionDeMisiones.Models;

[ApiController]
[Route("api/[controller]")]
public class Query6Controller : ControllerBase
{
    private readonly IQuery6Service _service;

    public Query6Controller(IQuery6Service service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Query6Result>>> GetRelacionHechiceroDiscipulos()
    {
        var result = await _service.GetRelacionHechiceroDiscipulosAsync();
        return Ok(result);
    }
}