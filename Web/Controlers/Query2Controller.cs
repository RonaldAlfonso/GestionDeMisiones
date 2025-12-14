using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.IService;
using GestionDeMisiones.Models;

[ApiController]
[Route("api/[controller]")]
public class Query2Controller : ControllerBase
{
    private readonly IQuery2Service _service;

    public Query2Controller(IQuery2Service service)
    {
        _service = service;
    }

    [HttpGet("hechicero/{hechiceroId}")]
    public async Task<ActionResult<IEnumerable<Query2Result>>> GetMisionesPorHechicero(int hechiceroId)
    {
        try
        {
            var result = await _service.GetMisionesPorHechiceroAsync(hechiceroId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}