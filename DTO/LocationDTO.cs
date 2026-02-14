using System;

namespace SistemaDeEventos.DTO;

public class LocationDTO
{
    public Guid Id { get; set; }

    public string Address { get; set; } = null!;

    public int Capacity { get; set; }
}
