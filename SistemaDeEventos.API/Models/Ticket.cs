using System;
using System.Collections.Generic;

namespace SistemaDeEventos.Models;

public partial class Ticket
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? UserId { get; set; }

    public int Quantity { get; set; }

    public decimal Value { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string? TicketType { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}
