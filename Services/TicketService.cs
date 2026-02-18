using System;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repository;

    public TicketService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketDTO?> GetByIdAsync(Guid id)
    {
        var ticket = await _repository.GetByIdAsync(id);
        if (ticket == null)
            return null;

        return MapToDTO(ticket);
    }

    public async Task<IEnumerable<TicketDTO>> GetAllAsync()
    {
        var tickets = await _repository.GetAllAsync();
        return tickets.Select(MapToDTO);
    }

    public async Task<IEnumerable<TicketDTO>> GetByUserIdAsync(Guid userId)
    {
        var tickets = await _repository.GetByUserIdAsync(userId);
        return tickets.Select(MapToDTO);
    }

    public async Task<IEnumerable<TicketDTO>> GetByOrderIdAsync(Guid orderId)
    {
        var tickets = await _repository.GetByOrderIdAsync(orderId);
        return tickets.Select(MapToDTO);
    }

    public async Task<IEnumerable<TicketDTO>> GetByAccessibilityAsync(bool? accessibility)
    {
        var tickets = await _repository.GetByAccessibilityAsync(accessibility);
        return tickets.Select(MapToDTO);
    }

    public async Task<TicketDTO> CreateAsync(TicketDTO ticketDTO)
    {
        var ticket = MapToModel(ticketDTO);
        ticket.Id = Guid.NewGuid();

        var created = await _repository.AddAsync(ticket);
        return MapToDTO(created);
    }

    public async Task<TicketDTO> UpdateAsync(Guid id, TicketDTO ticketDTO)
    {
        var ticket = await _repository.GetByIdAsync(id);
        if (ticket == null)
            throw new KeyNotFoundException($"Ingresso com ID {id} não encontrado.");

        ticket.OrderId = ticketDTO.OrderId;
        ticket.UserId = ticketDTO.UserId;
        ticket.Quantity = ticketDTO.Quantity;
        ticket.Value = ticketDTO.Value;
        ticket.Date = ticketDTO.Date;
        ticket.Time = ticketDTO.Time;
        ticket.TicketType = ticketDTO.TicketType;
        ticket.Accessibility = ticketDTO.Accessibility;

        var updated = await _repository.UpdateAsync(ticket);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private TicketDTO MapToDTO(Ticket ticket)
    {
        return new TicketDTO
        {
            Id = ticket.Id,
            OrderId = ticket.OrderId,
            UserId = ticket.UserId,
            EventId = ticket.EventId,
            Quantity = ticket.Quantity,
            Value = ticket.Value,
            Date = ticket.Date,
            Time = ticket.Time,
            TicketType = ticket.TicketType,
            Accessibility = ticket.Accessibility
        };
    }

    private Ticket MapToModel(TicketDTO ticketDTO)
    {
        return new Ticket
        {
            OrderId = ticketDTO.OrderId,
            UserId = ticketDTO.UserId,
            EventId = ticketDTO.EventId,
            Quantity = ticketDTO.Quantity,
            Value = ticketDTO.Value,
            Date = ticketDTO.Date,
            Time = ticketDTO.Time,
            TicketType = ticketDTO.TicketType,
            Accessibility = ticketDTO.Accessibility
        };
    }
}