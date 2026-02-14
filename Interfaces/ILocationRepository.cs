using System;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(Guid id);
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location> AddAsync(Location location);
    Task<Location> UpdateAsync(Location location);
    Task<bool> DeleteAsync(Guid id);
}
