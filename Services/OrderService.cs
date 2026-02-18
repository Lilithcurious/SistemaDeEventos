using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.DTOs.Order;

namespace SistemaDeEventos
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private static readonly string[] ValidPaymentTypes =
        {
            "CreditCard",
            "DebitCard",
            "Pix"
        };

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDTO> CreateOrder(
            Guid userId,
            decimal value,
            string paymentType)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId inválido.", nameof(userId));

            if (value <= 0)
                throw new ArgumentException("Valor deve ser maior que zero.", nameof(value));

            if (string.IsNullOrWhiteSpace(paymentType) ||
                !ValidPaymentTypes.Contains(paymentType))
            {
                throw new ArgumentException("Tipo de pagamento inválido.", nameof(paymentType));
            }

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
                UserId = order.UserId ?? Guid.Empty,
                CreatedAt = order.Created ?? DateTime.UtcNow
            };
        }

        public async Task<List<OrderResponseDTO>> GetOrders()
        {
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
