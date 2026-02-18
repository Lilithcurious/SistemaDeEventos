using SistemaDeEventos.Models;
using SistemaDeEventos.DTOs.Order;
using System.Threading.Tasks;
namespace SistemaDeEventos.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
}
