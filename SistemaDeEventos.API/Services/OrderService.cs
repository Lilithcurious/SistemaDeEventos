using SistemaDeEventos.Models;
using SistemaDeEventos.Services.Interface;
using SistemaDeEventosDTO;

namespace SistemaDeEventos 
{
    public class OrderService
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
                UserId = order.UserId.Value
            };
        }
    }
}