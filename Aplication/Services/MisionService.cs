using GestionDeMisiones.Models;
using GestionDeMisiones.IService;
using GestionDeMisiones.IRepository;
using Microsoft.EntityFrameworkCore;
using GestionDeMisiones.Data;

namespace GestionDeMisiones.Service;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repo;
    private readonly AppDbContext _context;

    public MisionService(IMisionRepository repo, AppDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<IEnumerable<Mision>> GetAllAsync()
        => await _repo.GetAllAsync();

    public async Task<Mision?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task<Mision> CreateAsync(Mision mision)
    {
        // Validaciones de negocio
        if (mision.FechaYHoraDeFin.HasValue && 
            mision.FechaYHoraDeFin <= mision.FechaYHoraDeInicio)
            throw new ArgumentException("La fecha de fin debe ser posterior a la de inicio");

        // Validar que la ubicación existe
        var ubicacionExists = await _context.Ubicaciones
            .AnyAsync(u => u.Id == mision.UbicacionId);
        if (!ubicacionExists)
            throw new ArgumentException("La ubicación especificada no existe");

        return await _repo.AddAsync(mision);
    }

    public async Task<bool> UpdateAsync(int id, Mision mision)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return false;

        // Validar que la ubicación existe
        var ubicacionExists = await _context.Ubicaciones
            .AnyAsync(u => u.Id == mision.UbicacionId);
        if (!ubicacionExists)
            throw new ArgumentException("La ubicación especificada no existe");

        // Validación de fechas
        if (mision.FechaYHoraDeFin.HasValue && 
            mision.FechaYHoraDeFin <= mision.FechaYHoraDeInicio)
            throw new ArgumentException("La fecha de fin debe ser posterior a la de inicio");

        // Actualizar solo campos permitidos
        existing.FechaYHoraDeInicio = mision.FechaYHoraDeInicio;
        existing.FechaYHoraDeFin = mision.FechaYHoraDeFin;
        existing.UbicacionId = mision.UbicacionId;
        existing.Estado = mision.Estado;
        existing.EventosOcurridos = mision.EventosOcurridos;
        existing.DannosColaterales = mision.DannosColaterales;
        existing.NivelUrgencia = mision.NivelUrgencia;

        await _repo.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return false;

        await _repo.DeleteAsync(existing);
        return true;
    }
}