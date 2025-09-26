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
        return Ok(_context.Hechiceros.ToList());
    }
    [HttpPost]
    public async Task<ActionResult<Hechicero>> newHechicero([FromBody] Hechicero hechicero)
    {
        _context.Hechiceros.Add(hechicero);
        await _context.SaveChangesAsync();
        return Ok();
    } 

}