using Microsoft.AspNetCore.Mvc;
using GestionDeMisiones.Models;
using GestionDeMisiones.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonalDeApoyoController : ControllerBase
{
    private readonly AppDbContext _context;
    public PersonalDeApoyoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PersonalDeApoyo>> GetPersonalDeApoyo()
    {
        return Ok(_context.PersonalDeApoyo.ToList());
    }
    [HttpGet("{id}")]
    public ActionResult<PersonalDeApoyo> GetPersonalDeApoyoById(int id)
    {
        var personal = _context.PersonalDeApoyo.FirstOrDefault(x => x.Id == id);
        if (personal == null)
        {
            return NotFound("El  personal de apoyo que busca no se encuentra.");
        }
        return Ok(personal);
    }
    [HttpPost]
    public async Task<ActionResult<PersonalDeApoyo>> PostPersonalDeApoyo([FromBody] PersonalDeApoyo personal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Envie un personal de apoyo valido");

        }
        await _context.PersonalDeApoyo.AddAsync(personal);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPersonalDeApoyoById), new { id = personal.Id }, personal);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonalDeApoyo>> PutPersonalDeApoyo(int id, [FromBody] PersonalDeApoyo personal)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest("envie un personal de apoyo valido");

        }
        var oldpersonal = _context.PersonalDeApoyo.FirstOrDefault(x => x.Id == id);
        if (oldpersonal == null)
        {
            return NotFound("El personal de apoyo que quiere editar no existe");
        }
        oldpersonal.Name = personal.Name;
        await _context.SaveChangesAsync();
        return NoContent();


    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<PersonalDeApoyo>> DeletePersonalDeApoyo(int id)
    {
        var personal = _context.PersonalDeApoyo.FirstOrDefault(x => x.Id == id);
        if (personal == null)
        {
            return NotFound("el personal que quiere eliminar no existe");
        }
        _context.PersonalDeApoyo.Remove(personal);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}