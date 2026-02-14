using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.Services.Interface;
using SistemaDeEventosDTO;

namespace SistemaDeEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder(
            [FromBody] OrderCreateRequestDTO request)
        {
            var order = await _orderService.CreateOrder(
                request.UserId,
                request.TotalAmount, // Use the correct property name from OrderCreateRequestDTO
                request.Quantity.ToString());

            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}

    
