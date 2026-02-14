using SistemaDeEventos;

namespace SistemaDeEventosDTO
{
    public class OrderCreateRequestDTO
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; internal set; }
    }

public class OrderResponseDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; } 
    public Guid EventId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderListResponseDTO
{
    public List<OrderResponseDTO> Orders { get; set; } = new();
}

}
