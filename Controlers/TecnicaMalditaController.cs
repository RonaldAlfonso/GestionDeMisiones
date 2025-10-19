using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicaMalditaController : ControllerBase
{
    private readonly AppDbContext _context;

    public TecnicaMalditaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TecnicaMaldita>> GetAllTecnicaMaldita()
    {
        return Ok(_context.TecnicasMalditas.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<TecnicaMaldita> GetTecnicaMaldita(int id)
    {
        var tecnica = _context.TecnicasMalditas.Find(id);
        if (tecnica == null)
        {
            return NotFound("La técnica maldita dada no existe en la base de datos");
        }
        return Ok(tecnica);
    }

    [HttpPost]
    public async Task<ActionResult<TecnicaMaldita>> PostTecnicaMaldita([FromBody] TecnicaMaldita tecnica)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("La descripción de la técnica maldita no cumple un formato correcto");
        }
        
        if (!Enum.IsDefined(typeof(TecnicaMaldita.ETipoTecnica), tecnica.Tipo))
        {
            return BadRequest("El tipo de técnica especificado no es válido");
        }

        tecnica.Id = 0;
        await _context.TecnicasMalditas.AddAsync(tecnica);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTecnicaMaldita), new { id = tecnica.Id }, tecnica);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TecnicaMaldita>> DeleteTecnicaMaldita(int id)
    {
        var tecnica = await _context.TecnicasMalditas.FindAsync(id);
        if (tecnica == null)
        {
            return NotFound("La técnica maldita dada no existe en la base de datos");
        }
        _context.TecnicasMalditas.Remove(tecnica);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TecnicaMaldita>> PutTecnicaMaldita(int id, TecnicaMaldita tecnica)
    {
        var tecnicaExistente = await _context.TecnicasMalditas.FindAsync(id);
        if (tecnicaExistente == null)
        {
            return NotFound("La técnica maldita dada no existe en la base de datos");
        }
        
        if (!Enum.IsDefined(typeof(TecnicaMaldita.ETipoTecnica), tecnica.Tipo))
        {
            return BadRequest("El tipo de técnica especificado no es válido");
        }

        tecnicaExistente.Nombre = tecnica.Nombre;
        tecnicaExistente.Tipo = tecnica.Tipo;
        tecnicaExistente.EfectividadProm = tecnica.EfectividadProm;
        tecnicaExistente.CondicionesDeUso = tecnica.CondicionesDeUso;

        await _context.SaveChangesAsync();
        return Ok();
    }
}