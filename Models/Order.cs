using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? Created { get; set; }

    public string? PaymentType { get; set; }

    public string? Status { get; set; }

    public decimal Value { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User? User { get; set; }
}
