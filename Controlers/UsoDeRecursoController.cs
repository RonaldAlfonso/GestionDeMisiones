using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsoDeRecursoController : ControllerBase
{
    private readonly AppDbContext _context;
    public UsoDeRecursoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UsoDeRecurso>> GetAllUsoDeRecurso()
    {
        return Ok(_context.UsosDeRecurso.Include(u => u.Mision).Include(u => u.Recurso).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<UsoDeRecurso> GetUsoDeRecurso(int id)
    {
        var usoDeRecurso = _context.UsosDeRecurso.Include(u => u.Mision)
                        .Include(u => u.Recurso).FirstOrDefault(u => u.Id == id);
        if (usoDeRecurso == null) return NotFound("El uso de recurso dado no existe");
        return Ok(usoDeRecurso);
    }

    [HttpPost]
    public async Task<ActionResult<UsoDeRecurso>> PostUsoDeRecurso([FromBody] UsoDeRecurso usoDeRecurso)
    {
        if (!ModelState.IsValid) return BadRequest("El uso de recurso dado no cumple el formato");

        bool enUso = _context.UsosDeRecurso.Any(u =>
            u.RecursoId == usoDeRecurso.RecursoId &&
            (usoDeRecurso.FechaFin == null
                ? u.FechaFin == null || usoDeRecurso.FechaInicio < u.FechaFin
                : usoDeRecurso.FechaInicio < u.FechaFin && usoDeRecurso.FechaFin > u.FechaInicio)
        );
        if (enUso) return Conflict("El recurso ya esta asignado en ese tiempo");

        await _context.UsosDeRecurso.AddAsync(usoDeRecurso);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsoDeRecurso), new { id = usoDeRecurso.Id }, usoDeRecurso);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UsoDeRecurso>> DeleteUsoDeRecurso(int id)
    {
        var usoDeRecurso = await _context.UsosDeRecurso.FindAsync(id);
        if (usoDeRecurso == null) return NotFound("El uso de recurso dado no existe");
        _context.UsosDeRecurso.Remove(usoDeRecurso);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UsoDeRecurso>> PutUsoDeRecurso([FromRoute] int id, [FromBody] UsoDeRecurso usoDeRecurso)
    {
        var _usoDeRecurso = await _context.UsosDeRecurso.FindAsync(id);
        if (_usoDeRecurso == null) return NotFound("El uso de recurso a modificar no existe");
        if (!ModelState.IsValid) return BadRequest("El uso de recurso dado no cumple el formato");

        bool enUso = _context.UsosDeRecurso.Any(u =>
            u.RecursoId == usoDeRecurso.RecursoId &&
            (usoDeRecurso.FechaFin == null
                ? u.FechaFin == null || usoDeRecurso.FechaInicio < u.FechaFin
                : usoDeRecurso.FechaInicio < u.FechaFin && usoDeRecurso.FechaFin > u.FechaInicio)
        );
        if (enUso) return Conflict("El recurso ya esta asignado en ese tiempo");

        _usoDeRecurso.MisionId = usoDeRecurso.MisionId;
        _usoDeRecurso.RecursoId = usoDeRecurso.RecursoId;
        _usoDeRecurso.FechaInicio = usoDeRecurso.FechaInicio;
        _usoDeRecurso.FechaFin = usoDeRecurso.FechaFin;
        _usoDeRecurso.Observaciones = usoDeRecurso.Observaciones;

        await _context.SaveChangesAsync();
        return Ok();
    }
}