using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketService _service;

    public TicketController(ITicketService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> Get([FromQuery] bool? accessibility = null)
    {
        if (accessibility.HasValue)
        {
            var filtered = await _service.GetByAccessibilityAsync(accessibility);
            return Ok(filtered);
        }

        var tickets = await _service.GetAllAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDTO>> Get(Guid id)
    {
        var ticket = await _service.GetByIdAsync(id);
        if (ticket == null)
            return NotFound();
        return Ok(ticket);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetByUserId(Guid userId)
    {
        var tickets = await _service.GetByUserIdAsync(userId);
        return Ok(tickets);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetByOrderId(Guid orderId)
    {
        var tickets = await _service.GetByOrderIdAsync(orderId);
        return Ok(tickets);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDTO>> Post([FromBody] TicketDTO ticketDTO)
    {
        var created = await _service.CreateAsync(ticketDTO);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] TicketDTO ticketDTO)
    {
        var updated = await _service.UpdateAsync(id, ticketDTO);
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
}
