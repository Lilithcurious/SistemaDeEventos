using System;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repository;

    public LocationService(ILocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<LocationDTO?> GetByIdAsync(Guid id)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location == null)
            return null;

        return MapToDTO(location);
    }

    public async Task<IEnumerable<LocationDTO>> GetAllAsync()
    {
        var locations = await _repository.GetAllAsync();
        return locations.Select(MapToDTO);
    }

    public async Task<LocationDTO> CreateAsync(LocationDTO locationDTO)
    {
        var location = MapToModel(locationDTO);
        location.Id = Guid.NewGuid();

        var created = await _repository.AddAsync(location);
        return MapToDTO(created);
    }

    public async Task<LocationDTO> UpdateAsync(Guid id, LocationDTO locationDTO)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location == null)
            throw new KeyNotFoundException($"Localização com ID {id} não encontrada.");

        location.Address = locationDTO.Address;
        location.Capacity = locationDTO.Capacity;

        var updated = await _repository.UpdateAsync(location);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private LocationDTO MapToDTO(Location location)
    {
        return new LocationDTO
        {
            Id = location.Id,
            Address = location.Address,
            Capacity = location.Capacity
        };
    }

    private Location MapToModel(LocationDTO locationDTO)
    {
        return new Location
        {
            Address = locationDTO.Address,
            Capacity = locationDTO.Capacity
        };
    }
}
