using System;

namespace SistemaDeEventos.DTOs.Order
{
    public class OrderCreateRequestDTO
    {
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; } = string.Empty;
    }

    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OrderListResponseDTO
    {
        public List<OrderResponseDTO> Orders { get; set; } = new();
    }

}
