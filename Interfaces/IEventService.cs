using System;
using SistemaDeEventos.DTO;

namespace SistemaDeEventos.Interfaces;

public interface IEventService
{
    Task<EventDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<EventDTO>> GetAllAsync();
    Task<EventDTO> CreateAsync(EventDTO eventDTO);
    Task<EventDTO> UpdateAsync(Guid id, EventDTO eventDTO);
    Task<bool> DeleteAsync(Guid id);
    Task<string> GetEventsReportCsvAsync();
}
