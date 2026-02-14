using System;

namespace SistemaDeEventos.Models;

public class Rating
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Comment { get; set; } = null!;
    public int Note { get; set; }

    // Foreign keys
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;
}




