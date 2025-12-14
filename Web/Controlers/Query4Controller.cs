using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.IService;
using GestionDeMisiones.Models;

[ApiController]
[Route("api/[controller]")]
public class Query4Controller : ControllerBase
{
    private readonly IQuery4Service _service;

    public Query4Controller(IQuery4Service service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Query4Result>>> GetEfectividadTecnicas()
    {
        var result = await _service.GetEfectividadTecnicasAsync();
        return Ok(result);
    }
}