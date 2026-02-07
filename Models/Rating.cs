using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class Rating
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public string? Comment { get; set; }

    public int? Note { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
