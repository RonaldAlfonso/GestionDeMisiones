using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TrasladoController : ControllerBase
{
    private readonly AppDbContext _context;
    public TrasladoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Traslado>> GetAllTransport()
    {
        return Ok(_context.Traslados.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Traslado> GetTrasladoById(int id)
    {
        var traslado = _context.Traslados.FirstOrDefault(x => x.Id == id);
        if (traslado == null)
        {
            return NotFound("El Traslado Solicitado no Existe");

        }
        return Ok(traslado);
    }

    [HttpPost]
    public async Task<ActionResult<Traslado>> PostTraslado([FromBody] Traslado traslado)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("El traslado que envia no es valido");
        }

        traslado.Id = 0;
        await _context.Traslados.AddAsync(traslado);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTrasladoById), new { id = traslado.Id }, traslado);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Traslado>> DeleteTraslado(int id)
    {
        var traslado = await _context.Traslados.FirstOrDefaultAsync(x => x.Id == id);
        if (traslado == null)
        {
            return NotFound("El Traslado que intento eliminar no existe");
        }
        _context.Traslados.Remove(traslado);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Traslado>> PutTraslado(int id, Traslado traslado)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("El traslado que envio no es valido");
        }
        var _traslado = await _context.Traslados.FirstOrDefaultAsync(x => x.Id == id);
        if (traslado == null)
        {
            return NotFound("El traslado que quiere modificar no existe");

        }
        _traslado.Destino = traslado.Destino;
        _traslado.Estado = traslado.Estado;
        _traslado.Fecha = traslado.Fecha;
        _traslado.Motivo = traslado.Motivo;
        _traslado.Origen = traslado.Origen;
        await _context.SaveChangesAsync();
        return NoContent();

    }

}