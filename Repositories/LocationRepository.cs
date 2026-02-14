using System;
using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly EventosContext _context;

    public LocationRepository(EventosContext context)
    {
        _context = context;
    }

    public async Task<Location?> GetByIdAsync(Guid id)
    {
        return await _context.Locations.FindAsync(id);
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    public async Task<Location> AddAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<Location> UpdateAsync(Location location)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null)
            return false;

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
        return true;
    }
}
