using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    // Relacionamentos
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}

