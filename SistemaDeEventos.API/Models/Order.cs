using System;

namespace SistemaDeEventos.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PaymentType { get; set; } = null!;
    public decimal Total { get; set; }

    // Foreign key
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}



