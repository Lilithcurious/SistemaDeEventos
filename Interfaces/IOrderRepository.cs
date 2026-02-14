using SistemaDeEventos.Models;
using SistemaDeEventosDTO;
using System.Threading.Tasks;
namespace SistemaDeEventos;

public interface IOrderRepository
{
    Task<OrderResponseDTO> CreateOrderAsync(Order order);
    Task<User?> GetUserByIdAsync(int userId);
    Task<Ticket?> GetTicketByIdAsync(int ticketId);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task AddAsync(Order order);
    Task AddQuantityAsync(Order order, int quantity);
    Task UpdateAsync(Order order);
    Task<IEnumerable<object>> GetAllAsync();
}
