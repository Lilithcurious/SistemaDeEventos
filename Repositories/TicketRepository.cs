using System;
using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly EventosContext _context;

    public TicketRepository(EventosContext context)
    {
        _context = context;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets.ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Tickets.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.Tickets.Where(t => t.OrderId == orderId).ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByAccessibilityAsync(bool? accessibility)
    {
        return await _context.Tickets.Where(t => t.Accessibility == accessibility).ToListAsync();
    }

    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return false;

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }
}
