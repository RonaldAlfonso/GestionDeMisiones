using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecursoController : ControllerBase
{
    private readonly AppDbContext _context;
    public RecursoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Recurso>> GetAllRecurso()
    {
        return Ok(_context.Recursos.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Recurso> GetRecurso(int id)
    {
        var recurso = _context.Recursos.Find(id);
        if (recurso == null) return NotFound("El recurso dado no existe");
        return Ok(recurso);
    }

    [HttpPost]
    public async Task<ActionResult<Recurso>> PostRecurso([FromBody] Recurso recurso)
    {
        if (!ModelState.IsValid) return BadRequest("El recurso dado no cumple el formato");
        recurso.Id = 0;
        await _context.Recursos.AddAsync(recurso);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRecurso), new { id = recurso.Id }, recurso);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Recurso>> DeleteRecurso(int id)
    {
        var recurso = await _context.Recursos.FindAsync(id);
        if (recurso == null) return NotFound("El recurso dado no existe");
        _context.Recursos.Remove(recurso);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Recurso>> PutRecurso([FromRoute] int id, [FromBody] Recurso recurso)
    {
        var _recurso = await _context.Recursos.FindAsync(id);
        if (_recurso == null) return NotFound("El recurso que se quiere modificar no existe");
        if (!ModelState.IsValid) return BadRequest("El recurso dado no cumple el formato");

        _recurso.Descripcion = recurso.Descripcion;
        _recurso.TipoRecurso = recurso.TipoRecurso;

        await _context.SaveChangesAsync();
        return Ok();
    }
}