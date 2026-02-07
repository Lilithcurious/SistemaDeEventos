using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class Location
{
    public Guid Id { get; set; }

    public string Address { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
