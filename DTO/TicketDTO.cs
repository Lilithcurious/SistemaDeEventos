using System;

namespace SistemaDeEventos.DTO;

public class TicketDTO
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? EventId { get; set; }

    public int Quantity { get; set; }

    public decimal Value { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string? TicketType { get; set; }
    public bool? Accessibility { get; set; }
}
