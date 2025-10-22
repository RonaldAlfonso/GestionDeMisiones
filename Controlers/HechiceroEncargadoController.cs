using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HechiceroEncargadoController:ControllerBase
{
    private readonly AppDbContext _context;

    public HechiceroEncargadoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<HechiceroEncargado>> GetAllHechiceroEncargado()
    {
        return Ok(_context.HechiceroEncargado.Include(h => h.Hechicero).Include(h => h.Mision).Include(h => h.Solicitud).ToList());
    }
    [HttpGet("{id}")]
    public ActionResult<HechiceroEncargado> GetHechiceroEncargadoById(int id)
    {
        var hechiceroEncargado = _context.HechiceroEncargado.Include(h=>h.Hechicero).Include(h=>h.Mision).Include(h=>h.Solicitud).FirstOrDefault(x => x.Id == id);
        if (hechiceroEncargado == null)
        {
            return NotFound("El hechicero que buscas no existe");
        }
        return Ok(hechiceroEncargado);
    }
    [HttpPost]
    public async Task<ActionResult<HechiceroEncargado>> PatchHechiceroEncargado([FromBody] HechiceroEncargado hechiceroEncargado)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie un hechicero encargado valido");
        }
        await _context.AddAsync(hechiceroEncargado);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHechiceroEncargadoById), new { id = hechiceroEncargado.Id }, hechiceroEncargado);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<HechiceroEncargado>> DeleteHechiceroEncargado(int id)
    {
        var hechiceroEncargado = _context.HechiceroEncargado.FirstOrDefault(x => x.Id == id);
        if (hechiceroEncargado == null)
        {
            return NotFound("El hechicero encargadoquebusca no existe");
        }
        _context.HechiceroEncargado.Remove(hechiceroEncargado);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<HechiceroEncargado>> UpdateMaldicion([FromRoute] int id, [FromBody] HechiceroEncargado hechiceroEncargado)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie  un hechicero encargado valido valida");
        }
        var hechiceroEncargadoOld = await _context.HechiceroEncargado.FindAsync(id);
        if (hechiceroEncargadoOld == null)
        {
            return NotFound("el hechicero encargado que desea editar no existe");
        }
        hechiceroEncargadoOld.Hechicero = hechiceroEncargado.Hechicero;
        hechiceroEncargadoOld.Mision = hechiceroEncargado.Mision;
        hechiceroEncargadoOld.Solicitud = hechiceroEncargado.Solicitud;
        await _context.SaveChangesAsync();
        return NoContent();

    }
} 