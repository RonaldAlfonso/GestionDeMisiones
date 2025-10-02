using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UbicacionController : ControllerBase
{
    private readonly AppDbContext _context;

    public UbicacionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Ubicacion>> GetAllUbicacion()
    {
        return Ok(_context.Ubicaciones.ToList());
    }
    [HttpGet("{id}")]
    public ActionResult<Ubicacion> GetUbicacion(int id)
    {
        var ubicacion = _context.Ubicaciones.Find(id);
        if(ubicacion == null)
        {
            return NotFound("La ubicacion dada no existe en la base de datos");
        }
        return Ok(ubicacion);
    }
    [HttpPost]
    public async Task<ActionResult<Ubicacion>> PostUbicacion([FromBody] Ubicacion ubicacion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("La descripcion de la ubicacion no cumple un formato correcto");
        }
        ubicacion.Id = 0;
        await _context.Ubicaciones.AddAsync(ubicacion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUbicacion), new { id = ubicacion.Id}, ubicacion);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Ubicacion>> DeleteUbicacion(int id)
    {
        var ubicacion = await _context.Ubicaciones.FindAsync(id);
        if (ubicacion == null)
        {
            return NotFound("La ubicacion dada no existe en la base de datos");
        }
        _context.Ubicaciones.Remove(ubicacion);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Ubicacion>> PutUbicacion(int id, Ubicacion ubicacion)
    {
        var ubicacionExistente = await _context.Ubicaciones.FindAsync(id);
        if (ubicacionExistente == null)
        {
            return NotFound("La ubicacion dada no existe en la base de datos");
        }
        ubicacionExistente.Nombre = ubicacion.Nombre;
        await _context.SaveChangesAsync();
        return Ok();
    }
}