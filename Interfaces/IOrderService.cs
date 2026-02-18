using SistemaDeEventos.DTOs.Order;

namespace SistemaDeEventos.Interfaces;

    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrder(
            Guid userId,
            decimal value,
            string paymentType);

        Task<List<OrderResponseDTO>> GetOrders();

        Task<OrderResponseDTO?> GetOrderById(Guid id);
    }
