using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]

public class HechiceroController : ControllerBase
{
    private readonly AppDbContext _context;

    public HechiceroController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Hechicero>> GetAllHechicero()
    {
        return Ok(_context.Hechiceros.Include(h => h.TecnicaPrincipal).ToList());
    }
    [HttpGet("{id}")]
    public ActionResult<Hechicero> GetHechiceroById(int id)
    {
        var hechicero = _context.Hechiceros.Include(h => h.TecnicaPrincipal).FirstOrDefault(x => x.Id == id);
        if (hechicero == null)
        {
            return NotFound("El hechicero que buscas no existe");

        }
        return Ok(hechicero);
    }
    [HttpPost]
    public async Task<ActionResult<Hechicero>> NewHechicero([FromBody] Hechicero hechicero)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie un hechicero valido");
        }
        hechicero.Id = 0;
        await _context.Hechiceros.AddAsync(hechicero);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHechiceroById), new { id = hechicero.Id }, hechicero);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Hechicero>> PutHechicero(int id, [FromBody] Hechicero newhechicero)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("envie un hechicero valido");

        }
        var hechicero = _context.Hechiceros.FirstOrDefault(x => x.Id == id);
        if (hechicero == null)
        {
            return NotFound("El hechicero que quiere editar no existe");
        }
        hechicero.Name = newhechicero.Name;
        hechicero.Estado = newhechicero.Estado;
        hechicero.Experiencia = newhechicero.Experiencia;
        hechicero.Grado = newhechicero.Grado;
        hechicero.TecnicaPrincipal = newhechicero.TecnicaPrincipal;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Hechicero>> DeleteHechicero(int id)
    {
        var hechicero = _context.Hechiceros.FirstOrDefault(x => x.Id == id);
        if (hechicero == null)
        {
            return NotFound("el hechiceroque quiere eliminar no existe");
        }
        _context.Hechiceros.Remove(hechicero);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}