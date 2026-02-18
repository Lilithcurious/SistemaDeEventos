using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class Rating
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    // navigation properties if needed
    public virtual User User { get; set; } = null!;
    // optionally keep order reference if still used
    public virtual Order? Order { get; set; }
}
