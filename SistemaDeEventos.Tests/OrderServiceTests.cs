using NUnit.Framework.Legacy;
using Moq;
using SistemaDeEventos;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.DTOs.Order;

public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockRepository;
    private OrderService _orderService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mockRepository.Object);
    }

    [Test]
    public async Task CreateOrder_DeveCriarPedido_QuandoDadosValidos()
    {
        var userId = Guid.NewGuid();
        decimal value = 100;
        string paymentType = "CreditCard";

        var result = await _orderService.CreateOrder(userId, value, paymentType);

       ClassicAssert.That(result.UserId, Is.EqualTo(userId));
       ClassicAssert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Test]
    public void CreateOrder_DeveLancarExcecao_QuandoValorMenorOuIgualZero()
    {
        var userId = Guid.NewGuid();

        Assert.ThrowsAsync<Exception>(async () =>
            await _orderService.CreateOrder(userId, 0, "Pix"));
    }

    [Test]
    public async Task GetOrderById_DeveRetornarOrder_QuandoExiste()
    {
        var id = Guid.NewGuid();
        var order = new Order
        {
            Id = id,
            UserId = Guid.NewGuid(),
            Created = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetOrderByIdAsync(id))
            .ReturnsAsync(order);

        var result = await _orderService.GetOrderById(id);

        ClassicAssert.That(result, Is.Not.Null);
        ClassicAssert.That(result.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetOrderById_DeveRetornarNull_QuandoNaoExiste()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetOrderByIdAsync(id))
            .ReturnsAsync((Order?)null);

        var result = await _orderService.GetOrderById(id);

        Assert.That(result, Is.Null);

    }
}
