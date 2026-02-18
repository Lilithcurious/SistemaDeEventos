using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class Event
{
    public Guid Id { get; set; }

    public string NameEvents { get; set; } = null!;

    public decimal Value { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public bool? Accessibility { get; set; }

    public Guid LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Ticket>? Tickets { get; set; }
}
