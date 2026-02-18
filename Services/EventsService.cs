using System;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventDTO?> GetByIdAsync(Guid id)
    {
        var @event = await _repository.GetByIdAsync(id);
        if (@event == null)
            return null;

        return MapToDTO(@event);
    }

    public async Task<IEnumerable<EventDTO>> GetAllAsync()
    {
        var events = await _repository.GetAllAsync();
        return events.Select(MapToDTO);
    }

    public async Task<IEnumerable<EventDTO>> GetByAccessibilityAsync(bool? accessibility)
    {
        var events = await _repository.GetByAccessibilityAsync(accessibility);
        return events.Select(MapToDTO);
    }

    public async Task<EventDTO> CreateAsync(EventDTO eventDTO)
    {
        var @event = MapToModel(eventDTO);
        @event.Id = Guid.NewGuid();

        var created = await _repository.AddAsync(@event);
        return MapToDTO(created);
    }

    public async Task<EventDTO> UpdateAsync(Guid id, EventDTO eventDTO)
    {
        var @event = await _repository.GetByIdAsync(id);
        if (@event == null)
            throw new KeyNotFoundException($"Evento com ID {id} não encontrado.");

        @event.NameEvents = eventDTO.NameEvents;
        @event.Value = eventDTO.Value;
        @event.Date = eventDTO.Date;
        @event.Time = eventDTO.Time;
        @event.Accessibility = eventDTO.Accessibility;
        @event.LocationId = eventDTO.LocationId;

        var updated = await _repository.UpdateAsync(@event);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<string> GetEventsReportCsvAsync()
    {
        return await _repository.GetEventsReportCsvAsync();
    }

    private EventDTO MapToDTO(Event @event)
    {
        return new EventDTO
        {
            Id = @event.Id,
            NameEvents = @event.NameEvents,
            Value = @event.Value,
            Date = @event.Date,
            Time = @event.Time,
            Accessibility = @event.Accessibility,
            LocationId = @event.LocationId
        };
    }

    private Event MapToModel(EventDTO eventDTO)
    {
        return new Event
        {
            NameEvents = eventDTO.NameEvents,
            Value = eventDTO.Value,
            Date = eventDTO.Date,
            Time = eventDTO.Time,
            Accessibility = eventDTO.Accessibility,
            LocationId = eventDTO.LocationId
        };
    }
}
