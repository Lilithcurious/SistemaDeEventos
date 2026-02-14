using System;
using SistemaDeEventos.DTO;

namespace SistemaDeEventos.Interfaces;

public interface ILocationService
{
    Task<LocationDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<LocationDTO>> GetAllAsync();
    Task<LocationDTO> CreateAsync(LocationDTO locationDTO);
    Task<LocationDTO> UpdateAsync(Guid id, LocationDTO locationDTO);
    Task<bool> DeleteAsync(Guid id);
}
