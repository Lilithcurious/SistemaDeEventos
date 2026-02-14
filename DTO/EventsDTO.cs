using System;

namespace SistemaDeEventos.DTO;

public class EventDTO
{
    public Guid Id { get; set; }

    public string NameEvents { get; set; } = null!;

    public decimal Value { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public bool? Accessibility { get; set; }

    public Guid LocationId { get; set; }
}
