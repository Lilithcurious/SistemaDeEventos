using System;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Interfaces;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id);
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Ticket>> GetByOrderIdAsync(Guid orderId);
    Task<IEnumerable<Ticket>> GetByAccessibilityAsync(string accessibility);
    Task<Ticket> AddAsync(Ticket ticket);
    Task<Ticket> UpdateAsync(Ticket ticket);
    Task<bool> DeleteAsync(Guid id);
}
