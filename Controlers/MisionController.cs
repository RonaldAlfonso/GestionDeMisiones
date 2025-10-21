using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MisionController : ControllerBase
{
    private readonly AppDbContext _context;
    public MisionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Mision>> GetAllMision()
    {
        return Ok(_context.Misiones.Include(m => m.Ubicacion).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Mision> GetMision(int id)
    {
        var mision = _context.Misiones.Include(m => m.Ubicacion).FirstOrDefault(x => x.Id == id);
        if (mision == null) return NotFound("La maldicion dada no existe");
        return Ok(mision);
    }

    [HttpPost]
    public async Task<ActionResult<Mision>> PostMision([FromBody] Mision mision)
    {
        if (!ModelState.IsValid) return BadRequest("La mision dada no cumple el formato");
        if (mision.FechaYHoraDeFin.HasValue &&
            mision.FechaYHoraDeFin <= mision.FechaYHoraDeInicio)
        {
            return BadRequest("La fecha de fin debe ser posterior a la de inicio");
        }

        mision.Id = 0;
        await _context.Misiones.AddAsync(mision);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMision), new { id = mision.Id }, mision);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Mision>> DeleteMision(int id)
    {
        var mision = await _context.Misiones.FindAsync(id);
        if (mision == null) return NotFound("La mision dada no existe");
        _context.Misiones.Remove(mision);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Mision>> PutMision([FromRoute] int id, [FromBody] Mision mision)
    {   
        if (!ModelState.IsValid) return BadRequest("La mision dada no cumple el formato");
        var _mision = await _context.Misiones.FindAsync(id);
        if (_mision == null) return NotFound("La mision que se quiere modificar no existe");
        var _ubicacion = await _context.Ubicaciones.FindAsync(mision.UbicacionId);
        if (_ubicacion == null) return BadRequest("La ubicacion de la mision dada no existe");
        if (mision.FechaYHoraDeFin.HasValue &&
            mision.FechaYHoraDeFin <= mision.FechaYHoraDeInicio)
        {
            return BadRequest("La fecha de fin debe ser posterior a la de inicio");
        }

        _mision.DannosColaterales = mision.DannosColaterales;
        _mision.Estado = mision.Estado;
        _mision.EventosOcurridos = mision.EventosOcurridos;
        _mision.FechaYHoraDeInicio = mision.FechaYHoraDeInicio;
        _mision.FechaYHoraDeFin = mision.FechaYHoraDeFin;
        _mision.NivelUrgencia = mision.NivelUrgencia;
        _mision.UbicacionId = mision.UbicacionId;

        await _context.SaveChangesAsync();
        return Ok();
    }
}