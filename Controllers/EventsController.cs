using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDTO>>> Get([FromQuery] bool? accessibility = null)
    {
        if (accessibility.HasValue)
        {
            var filtered = await _service.GetByAccessibilityAsync(accessibility);
            return Ok(filtered);
        }

        var events = await _service.GetAllAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> Get(Guid id)
    {
        var @event = await _service.GetByIdAsync(id);
        if (@event == null)
            return NotFound();
        return Ok(@event);
    }

    [HttpPost]
    public async Task<ActionResult<EventDTO>> Post([FromBody] EventDTO eventDTO)
    {
        var created = await _service.CreateAsync(eventDTO);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] EventDTO eventDTO)
    {
        var updated = await _service.UpdateAsync(id, eventDTO);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Gera relatório CSV com todos os eventos cadastrados
    /// </summary>
    [HttpGet("relatorio")]
    public async Task<IActionResult> GetRelatorio()
    {
        var csv = await _service.GetEventsReportCsvAsync();
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
        return File(bytes, "text/csv", $"eventos_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
    }
}
