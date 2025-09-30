using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaldicionController : ControllerBase
{
    private readonly AppDbContext _context;

    public MaldicionController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Maldicion>> GetAllMalditions()
    {
        return Ok(_context.Maldiciones.ToList());
    }
    [HttpGet("{id}")]
    public ActionResult<Maldicion> GetMaldition(int id)
    {
        var maldition = _context.Maldiciones.Find(id);
        if (maldition == null)
        {
            return NotFound("La maldicion que buscas no existe");

        }
        return Ok(maldition);
    }
    [HttpPost]
    public async Task<ActionResult<Maldicion>> PostMaldition([FromBody] Maldicion maldicion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie una Maldicion valida");
        }
        maldicion.Id = 0;
        await _context.Maldiciones.AddAsync(maldicion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMaldition), new { id = maldicion.Id }, maldicion);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Maldicion>> DeleteMaldicion(int id)
    {
        var maldicion = await _context.Maldiciones.FindAsync(id);
        if (maldicion == null)
        {
            return NotFound("La Maldicion que deseas eliminar no existe");
        }
        _context.Maldiciones.Remove(maldicion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Maldicion>> UpdateMaldicion([FromRoute] int id, [FromBody] Maldicion maldicion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie  una maldicion valida");
        }
        var maldicionVieja = await _context.Maldiciones.FindAsync(id);
        if (maldicionVieja == null)
        {
            return NotFound("la maldicion que desea editar no existe");
        }
        maldicionVieja.Nombre = maldicion.Nombre;
        maldicionVieja.EstadoActual = maldicion.EstadoActual;
        maldicionVieja.FechaYHoraDeAparicion = maldicion.FechaYHoraDeAparicion;
        maldicionVieja.Grado = maldicion.Grado;
        maldicionVieja.NivelPeligro = maldicion.NivelPeligro;
        maldicionVieja.Tipo = maldicion.Tipo;
        await _context.SaveChangesAsync();
        return NoContent();

    }
} 