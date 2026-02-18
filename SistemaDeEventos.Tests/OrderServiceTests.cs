using NUnit.Framework;
using Moq;
using SistemaDeEventos;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;

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
        // Arrange
        var userId = Guid.NewGuid();
        var value = 100m;
        var paymentType = "CreditCard";

        // Act
        var result = await _orderService.CreateOrder(userId, value, paymentType);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Test]
    public void CreateOrder_DeveLancarExcecao_QuandoUserIdVazio()
    {
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _orderService.CreateOrder(Guid.Empty, 100m, "CreditCard"));

        Assert.That(exception!.ParamName, Is.EqualTo("userId"));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void CreateOrder_DeveLancarExcecao_QuandoValorMenorOuIgualZero()
    {
        var userId = Guid.NewGuid();

        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _orderService.CreateOrder(userId, 0m, "Pix"));

        Assert.That(exception!.ParamName, Is.EqualTo("value"));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void CreateOrder_DeveLancarExcecao_QuandoPaymentTypeInvalido()
    {
        var userId = Guid.NewGuid();

        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _orderService.CreateOrder(userId, 100m, "Invalido"));

        Assert.That(exception!.ParamName, Is.EqualTo("paymentType"));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public async Task GetOrderById_DeveRetornarOrder_QuandoExiste()
    {
        // Arrange
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

        // Act
        var result = await _orderService.GetOrderById(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(id));

        _mockRepository.Verify(r => r.GetOrderByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetOrderById_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetOrderByIdAsync(id))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.GetOrderById(id);

        // Assert
        Assert.That(result, Is.Null);

        _mockRepository.Verify(r => r.GetOrderByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetOrders_DeveRetornarListaVazia()
    {
        // Act
        var result = await _orderService.GetOrders();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
}
