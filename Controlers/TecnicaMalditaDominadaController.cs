using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicaMalditaDominadaController : ControllerBase
{
    private readonly AppDbContext _context;

    public TecnicaMalditaDominadaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TecnicaMalditaDominada>> GetAllTecnicaMalditaDominada()
    {
        return Ok(_context.TecnicasMalditasDominadas
            .Include(tmd => tmd.Hechicero)
            .Include(tmd => tmd.TecnicaMaldita)
            .ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<TecnicaMalditaDominada> GetTecnicaMalditaDominada(int id)
    {
        var tecnicaDominada = _context.TecnicasMalditasDominadas
            .Include(tmd => tmd.Hechicero)
            .Include(tmd => tmd.TecnicaMaldita)
            .FirstOrDefault(tmd => tmd.Id == id);
        
        if (tecnicaDominada == null) 
            return NotFound("La técnica maldita dominada dada no existe");
        
        return Ok(tecnicaDominada);
    }

    [HttpPost]
    public async Task<ActionResult<TecnicaMalditaDominada>> PostTecnicaMalditaDominada([FromBody] TecnicaMalditaDominada tecnicaDominada)
    {
        if (!ModelState.IsValid) 
            return BadRequest("La técnica maldita dominada no cumple el formato");

        var hechicero = await _context.Hechiceros.FindAsync(tecnicaDominada.HechiceroId);
        if (hechicero == null)
            return BadRequest("El hechicero especificado no existe");

        var tecnicaMaldita = await _context.TecnicasMalditas.FindAsync(tecnicaDominada.TecnicaMalditaId);
        if (tecnicaMaldita == null)
            return BadRequest("La técnica maldita especificada no existe");

        if (tecnicaDominada.NivelDeDominio < 0 || tecnicaDominada.NivelDeDominio > 100)
            return BadRequest("El nivel de dominio debe estar entre 0 y 100");

        var existeRelacion = await _context.TecnicasMalditasDominadas
            .AnyAsync(tmd => tmd.HechiceroId == tecnicaDominada.HechiceroId && 
                            tmd.TecnicaMalditaId == tecnicaDominada.TecnicaMalditaId);
        
        if (existeRelacion)
            return BadRequest("Este hechicero ya domina esta técnica maldita");

        tecnicaDominada.Id = 0;
        await _context.TecnicasMalditasDominadas.AddAsync(tecnicaDominada);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTecnicaMalditaDominada), new { id = tecnicaDominada.Id }, tecnicaDominada);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TecnicaMalditaDominada>> DeleteTecnicaMalditaDominada(int id)
    {
        var tecnicaDominada = await _context.TecnicasMalditasDominadas.FindAsync(id);
        if (tecnicaDominada == null) 
            return NotFound("La técnica maldita dominada dada no existe");
        
        _context.TecnicasMalditasDominadas.Remove(tecnicaDominada);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TecnicaMalditaDominada>> PutTecnicaMalditaDominada([FromRoute] int id, [FromBody] TecnicaMalditaDominada tecnicaDominada)
    {
        if (!ModelState.IsValid) 
            return BadRequest("La técnica maldita dominada no cumple el formato");

        var tecnicaDominadaExistente = await _context.TecnicasMalditasDominadas.FindAsync(id);
        if (tecnicaDominadaExistente == null) 
            return NotFound("La técnica maldita dominada que se quiere modificar no existe");

        if (tecnicaDominadaExistente.HechiceroId != tecnicaDominada.HechiceroId)
        {
            var hechicero = await _context.Hechiceros.FindAsync(tecnicaDominada.HechiceroId);
            if (hechicero == null)
                return BadRequest("El hechicero especificado no existe");
        }

        if (tecnicaDominadaExistente.TecnicaMalditaId != tecnicaDominada.TecnicaMalditaId)
        {
            var tecnicaMaldita = await _context.TecnicasMalditas.FindAsync(tecnicaDominada.TecnicaMalditaId);
            if (tecnicaMaldita == null)
                return BadRequest("La técnica maldita especificada no existe");
        }

        if (tecnicaDominada.NivelDeDominio < 0 || tecnicaDominada.NivelDeDominio > 100)
            return BadRequest("El nivel de dominio debe estar entre 0 y 100");

        if (tecnicaDominadaExistente.HechiceroId != tecnicaDominada.HechiceroId || 
            tecnicaDominadaExistente.TecnicaMalditaId != tecnicaDominada.TecnicaMalditaId)
        {
            var existeDuplicado = await _context.TecnicasMalditasDominadas
                .AnyAsync(tmd => tmd.Id != id &&
                                tmd.HechiceroId == tecnicaDominada.HechiceroId && 
                                tmd.TecnicaMalditaId == tecnicaDominada.TecnicaMalditaId);
            
            if (existeDuplicado)
                return BadRequest("Ya existe esta combinación de hechicero y técnica maldita");
        }

        tecnicaDominadaExistente.HechiceroId = tecnicaDominada.HechiceroId;
        tecnicaDominadaExistente.TecnicaMalditaId = tecnicaDominada.TecnicaMalditaId;
        tecnicaDominadaExistente.NivelDeDominio = tecnicaDominada.NivelDeDominio;

        await _context.SaveChangesAsync();
        return Ok();
    }
}