using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly EventosContext _db;

    public LocationController(EventosContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> Get()
    {
        return await _db.Locations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> Get(Guid id)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location == null) return NotFound();
        return location;
    }

    [HttpPost]
    public async Task<ActionResult<Location>> Post(Location location)
    {
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Location location)
    {
        if (id != location.Id) return BadRequest();

        _db.Entry(location).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LocationExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location == null) return NotFound();

        _db.Locations.Remove(location);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool LocationExists(Guid id) => _db.Locations.Any(e => e.Id == id);
}

