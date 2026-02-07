using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
