using System;
using SistemaDeEventos.DTO;

namespace SistemaDeEventos.Interfaces;

public interface ITicketService
{
    Task<TicketDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<TicketDTO>> GetAllAsync();
    Task<IEnumerable<TicketDTO>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<TicketDTO>> GetByOrderIdAsync(Guid orderId);
    Task<IEnumerable<TicketDTO>> GetByAccessibilityAsync(string accessibility);
    Task<TicketDTO> CreateAsync(TicketDTO ticketDTO);
    Task<TicketDTO> UpdateAsync(Guid id, TicketDTO ticketDTO);
    Task<bool> DeleteAsync(Guid id);
}
