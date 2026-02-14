using SistemaDeEventos.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using SistemaDeEventosDTO;
namespace SistemaDeEventos.Repository;

public class OrderRepository
{
    private readonly EventosContext _context;

    public OrderRepository(EventosContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDTO> CreateOrderAsync(Order order)
    {
        var entry = await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        var savedOrder = entry.Entity;
        return new OrderResponseDTO
        {
            OrderId = savedOrder.Id,
            UserId = savedOrder.UserId ?? Guid.Empty,
            Quantity = savedOrder.Quantity
        };
    }
}

