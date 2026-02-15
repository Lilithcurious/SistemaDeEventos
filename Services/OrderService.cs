using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.DTOs.Order;

namespace SistemaDeEventos 
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDTO> CreateOrder(
            Guid userId,
            decimal value,
            string paymentType)
        {
            if (value <= 0)
                throw new Exception("Valor deve ser maior que zero.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Created = DateTime.UtcNow,
                PaymentType = paymentType,
                Status = "Created",
                Value = value
            };

            await _orderRepository.AddAsync(order);

            return new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId.Value,
                CreatedAt = order.Created ?? DateTime.UtcNow
            };
        }

        public async Task<List<OrderResponseDTO>> GetOrders()
        {
            // simple stub; would ideally map repository results
            return new List<OrderResponseDTO>();
        }

        public async Task<OrderResponseDTO?> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return null;

            return new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId ?? Guid.Empty,
                CreatedAt = order.Created ?? DateTime.UtcNow
            };
        }
    }
}