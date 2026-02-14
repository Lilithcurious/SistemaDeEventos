using System;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event> AddAsync(Event @event);
    Task<Event> UpdateAsync(Event @event);
    Task<bool> DeleteAsync(Guid id);
    Task<string> GetEventsReportCsvAsync();
}
