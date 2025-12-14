using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.IService;


namespace GestionDeMisiones.Controllers;

    
[ApiController]
[Route("api/[controller]")]
public class RankingHechicerosController : ControllerBase
{
    private readonly IRankingHechiceroService _service;

    public RankingHechicerosController(IRankingHechiceroService service)
    {
        _service = service;
    }

    [HttpGet("top-por-nivel")]
    public async Task<IActionResult> GetRankingPorNivel([FromQuery] int ubicacionId)
    {
        var resultado = await _service.GetTopHechicerosPorNivelYUbicacion(ubicacionId);
        return Ok(resultado);
    }
}
