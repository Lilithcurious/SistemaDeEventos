using SistemaDeEventos.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using SistemaDeEventos.DTOs.Order;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Repositories;

public class OrderRepository : IOrderRepository
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
            Id = savedOrder.Id,
            UserId = savedOrder.UserId ?? Guid.Empty,
            CreatedAt = savedOrder.Created ?? DateTime.UtcNow
        };
    }

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders.FindAsync(orderId);
    }
}

