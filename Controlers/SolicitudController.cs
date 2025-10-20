using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SolicitudController : ControllerBase
{
    private readonly AppDbContext _context;

    public SolicitudController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Solicitud>> GetAllSolicitud()
    {
        return Ok(_context.Solicitud.Include(h => h.Maldicion).ThenInclude(m=>m.UbicacionDeAparicion).ToList());
    }
    [HttpGet("{id}")]
    public ActionResult<Solicitud> GetSolicitudById(int id)
    {
        var solicitud = _context.Solicitud.Include(h => h.Maldicion).ThenInclude(m=>m.UbicacionDeAparicion).FirstOrDefault(x => x.Id == id);
        if (solicitud == null)
        {
            return NotFound("La Solicitud que buscas no existe");

        }
        return Ok(solicitud);
    }
    [HttpPost]
    public async Task<ActionResult<Solicitud>> NewSolicitud([FromBody] Solicitud solicitud)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie una Solicitud valida");
        }
        solicitud.Id = 0;
        await _context.Solicitud.AddAsync(solicitud);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSolicitudById), new { id = solicitud.Id }, solicitud);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Solicitud>> PutSolicitud(int id, [FromBody] Solicitud newSolicitud)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("envie un Solicitud valido");

        }
        var solicitud = _context.Solicitud.FirstOrDefault(x => x.Id == id);
        if (solicitud == null)
        {
            return NotFound("El Solicitud que quiere editar no existe");
        }
        solicitud.Maldicion = newSolicitud.Maldicion;
        solicitud.Estado = newSolicitud.Estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Solicitud>> DeleteSolicitud(int id)
    {
        var solicitud = _context.Solicitud.FirstOrDefault(x => x.Id == id);
        if (solicitud == null)
        {
            return NotFound("La Solicitud que quiere eliminar no existe");
        }
        _context.Solicitud.Remove(solicitud);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}