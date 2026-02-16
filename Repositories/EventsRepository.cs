using System;
using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Repositories;

public class EventsRepository : IEventRepository
{
    private readonly EventosContext _context;

    public EventsRepository(EventosContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByAccessibilityAsync(bool? accessibility)
    {
        return await _context.Events.Where(e => e.Accessibility == accessibility).ToListAsync();
    }

    public async Task<Event> AddAsync(Event @event)
    {
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task<Event> UpdateAsync(Event @event)
    {
        _context.Events.Update(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
            return false;

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Gera relatório CSV com todos os eventos
    /// </summary>
    public async Task<string> GetEventsReportCsvAsync()
    {
            // include location to demonstrate JOIN operation
            var events = await _context.Events
                .Include(e => e.Location)
                .ToListAsync();

            var csv = new System.Text.StringBuilder();
            // Cabeçalho (inclui endereço da localização)
            csv.AppendLine("ID,Nome,Valor,Data,Hora,Acessibilidade,LocalizacaoID,LocalizacaoEndereco");
        foreach (var evt in events)
        {
            var accessibility = evt.Accessibility.HasValue ? (evt.Accessibility.Value ? "Sim" : "Não") : "N/A";
            var locationAddress = evt.Location != null ? EscapeCsv(evt.Location.Address) : "";
            csv.AppendLine($"\"{evt.Id}\",\"{EscapeCsv(evt.NameEvents)}\",{evt.Value:F2},{evt.Date:yyyy-MM-dd},{evt.Time:HH:mm:ss},{accessibility},\"{evt.LocationId}\",\"{locationAddress}\"");
        }

        return csv.ToString();
    }

    private string EscapeCsv(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            return value.Replace("\"", "\\\"");
        return value;
    }
}
