using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTOs.Order;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Tests.Controllers;

[TestFixture]
public class OrderControllerTests
{
    private static OrderCreateRequestDTO CreateRequest()
        => new()
        {
            UserId = Guid.NewGuid(),
            TotalAmount = 199.90m,
            PaymentType = "PIX"
        };

    private static OrderResponseDTO CreateResponse(Guid? id = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            TotalAmount = 199.90m,
            PaymentType = "PIX",
            CreatedAt = DateTime.UtcNow
        };

    [Test]
    public async Task CreateOrder_DeveRetornarOkComOrder()
    {
        // Arrange
        var request = CreateRequest();

        var created = new OrderResponseDTO
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            TotalAmount = request.TotalAmount,
            PaymentType = request.PaymentType,
            CreatedAt = DateTime.UtcNow
        };

        var service = new Mock<IOrderService>();
        service.Setup(s => s.CreateOrder(request.UserId, request.TotalAmount, request.PaymentType))
               .ReturnsAsync(created);

        var controller = new OrderController(service.Object);

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as OrderResponseDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(created.Id));
        Assert.That(dto.UserId, Is.EqualTo(request.UserId));
        Assert.That(dto.TotalAmount, Is.EqualTo(request.TotalAmount));
        Assert.That(dto.PaymentType, Is.EqualTo(request.PaymentType));

        service.Verify(s => s.CreateOrder(request.UserId, request.TotalAmount, request.PaymentType), Times.Once);
        service.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetOrderById_QuandoExistir_DeveRetornarOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var order = CreateResponse(id);

        var service = new Mock<IOrderService>();
        service.Setup(s => s.GetOrderById(id))
               .ReturnsAsync(order);

        var controller = new OrderController(service.Object);

        // Act
        var result = await controller.GetOrderById(id);

        // Assert
        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as OrderResponseDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));

        service.Verify(s => s.GetOrderById(id), Times.Once);
        service.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetOrderById_QuandoNaoExistir_DeveRetornarNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        var service = new Mock<IOrderService>();
        service.Setup(s => s.GetOrderById(id))
               .ReturnsAsync((OrderResponseDTO?)null);

        var controller = new OrderController(service.Object);

        // Act
        var result = await controller.GetOrderById(id);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());

        service.Verify(s => s.GetOrderById(id), Times.Once);
        service.VerifyNoOtherCalls();
    }
}
