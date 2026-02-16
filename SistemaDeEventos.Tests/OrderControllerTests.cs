using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTOs.Order;
using SistemaDeEventos.Interfaces;
using System;
using System.Threading.Tasks;

namespace SistemaDeEventos.Tests.Controllers
{
    public class OrderControllerTests
    {
        private Mock<IOrderService> _mockOrderService;
        private OrderController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
        }

        [Test]
        public async Task CreateOrder_ValidRequest_ReturnsOk()
        {
            var request = new OrderCreateRequestDTO
            {
                UserId = Guid.NewGuid(),
                TotalAmount = 150m,
                PaymentType = "Pix"
            };

            var response = new OrderResponseDTO
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _mockOrderService
                .Setup(s => s.CreateOrder(request.UserId, request.TotalAmount, request.PaymentType))
                .ReturnsAsync(response);

            var result = await _controller.CreateOrder(request);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;
            var order = (OrderResponseDTO)okResult.Value!;

            Assert.That(order.UserId, Is.EqualTo(request.UserId));
        }

        [Test]
        public async Task GetOrderById_ExistingOrder_ReturnsOk()
        {
            var orderId = Guid.NewGuid();

            var response = new OrderResponseDTO
            {
                Id = orderId,
                UserId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            _mockOrderService
                .Setup(s => s.GetOrderById(orderId))
                .ReturnsAsync(response);

            var result = await _controller.GetOrderById(orderId);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;
            var order = (OrderResponseDTO)okResult.Value!;

            Assert.That(order.Id, Is.EqualTo(orderId));
        }

        [Test]
        public async Task GetOrderById_NonExistingOrder_ReturnsNotFound()
        {
            var orderId = Guid.NewGuid();

            _mockOrderService
                .Setup(s => s.GetOrderById(orderId))
                .ReturnsAsync((OrderResponseDTO?)null);

            var result = await _controller.GetOrderById(orderId);

            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void CreateOrder_ServiceThrowsException_ShouldPropagate()
        {
            var request = new OrderCreateRequestDTO
            {
                UserId = Guid.NewGuid(),
                TotalAmount = 0,
                PaymentType = "Pix"
            };

            _mockOrderService
                .Setup(s => s.CreateOrder(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Valor deve ser maior que zero."));

            Assert.ThrowsAsync<Exception>(async () =>
                await _controller.CreateOrder(request));
        }
    }
}

