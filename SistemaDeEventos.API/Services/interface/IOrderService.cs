using SistemaDeEventosDTO;

namespace SistemaDeEventos.Services.Interface;

    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrder(
            Guid userId,
            decimal value,
            string paymentType);

        Task<List<OrderResponseDTO>> GetOrders();

        Task<OrderResponseDTO?> GetOrderById(Guid id);
    Task CreateOrder(Guid userId, Guid eventId, int quantity);
}

