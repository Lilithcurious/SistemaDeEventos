using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Tests.Controllers;

[TestFixture]
public class TicketControllerTests
{
    private static TicketDTO CreateDto(Guid? id = null)
    {
        return new TicketDTO
        {
            Id = id ?? Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Quantity = 1,
            Value = 50,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Time = new TimeOnly(18, 30),
            TicketType = "Pista",
            Accessibility = true
        };
    }

    [Test]
    public async Task Get_SemFiltro_DeveRetornarOkComLista()
    {
        var service = new Mock<ITicketService>();
        service.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<TicketDTO> { CreateDto(), CreateDto() });

        var controller = new TicketController(service.Object);

        var result = await controller.Get(null);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var data = ok!.Value as IEnumerable<TicketDTO>;
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Count(), Is.EqualTo(2));

        service.Verify(s => s.GetAllAsync(), Times.Once);
        service.Verify(s => s.GetByAccessibilityAsync(It.IsAny<bool?>()), Times.Never);
    }

    [Test]
    public async Task Get_ComFiltroAccessibility_DeveRetornarOkComListaFiltrada()
    {
        var service = new Mock<ITicketService>();
        service.Setup(s => s.GetByAccessibilityAsync(true))
            .ReturnsAsync(new List<TicketDTO> { CreateDto() });

        var controller = new TicketController(service.Object);

        var result = await controller.Get(true);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var data = ok!.Value as IEnumerable<TicketDTO>;
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Count(), Is.EqualTo(1));

        service.Verify(s => s.GetByAccessibilityAsync(true), Times.Once);
        service.Verify(s => s.GetAllAsync(), Times.Never);
    }

    [Test]
    public async Task GetPorId_DeveRetornarOk_QuandoExistir()
    {
        var id = Guid.NewGuid();

        var service = new Mock<ITicketService>();
        service.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(CreateDto(id));

        var controller = new TicketController(service.Object);

        var result = await controller.Get(id);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as TicketDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));

        service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetPorId_DeveRetornarNotFound_QuandoNaoExistir()
    {
        var id = Guid.NewGuid();

        var service = new Mock<ITicketService>();
        service.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((TicketDTO?)null);

        var controller = new TicketController(service.Object);

        var result = await controller.Get(id);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());

        service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetByUserId_DeveRetornarOkComLista()
    {
        var userId = Guid.NewGuid();

        var service = new Mock<ITicketService>();
        service.Setup(s => s.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<TicketDTO> { CreateDto(), CreateDto() });

        var controller = new TicketController(service.Object);

        var result = await controller.GetByUserId(userId);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var data = ok!.Value as IEnumerable<TicketDTO>;
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Count(), Is.EqualTo(2));

        service.Verify(s => s.GetByUserIdAsync(userId), Times.Once);
    }

    [Test]
    public async Task GetByOrderId_DeveRetornarOkComLista()
    {
        var orderId = Guid.NewGuid();

        var service = new Mock<ITicketService>();
        service.Setup(s => s.GetByOrderIdAsync(orderId))
            .ReturnsAsync(new List<TicketDTO> { CreateDto() });

        var controller = new TicketController(service.Object);

        var result = await controller.GetByOrderId(orderId);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var data = ok!.Value as IEnumerable<TicketDTO>;
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Count(), Is.EqualTo(1));

        service.Verify(s => s.GetByOrderIdAsync(orderId), Times.Once);
    }

    [Test]
    public async Task Post_DeveRetornarCreatedAtAction()
    {
        var created = CreateDto(Guid.NewGuid());

        var service = new Mock<ITicketService>();
        service.Setup(s => s.CreateAsync(It.IsAny<TicketDTO>()))
            .ReturnsAsync(created);

        var controller = new TicketController(service.Object);

        var result = await controller.Post(CreateDto());

        var createdAt = result.Result as CreatedAtActionResult;
        Assert.That(createdAt, Is.Not.Null);
        Assert.That(createdAt!.ActionName, Is.EqualTo("Get"));

        var dto = createdAt.Value as TicketDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(created.Id));

        service.Verify(s => s.CreateAsync(It.IsAny<TicketDTO>()), Times.Once);
    }

    [Test]
    public async Task Put_DeveRetornarOkComAtualizado()
    {
        var id = Guid.NewGuid();
        var updated = CreateDto(id);

        var service = new Mock<ITicketService>();
        service.Setup(s => s.UpdateAsync(id, It.IsAny<TicketDTO>()))
            .ReturnsAsync(updated);

        var controller = new TicketController(service.Object);

        var result = await controller.Put(id, CreateDto());

        var ok = result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as TicketDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));

        service.Verify(s => s.UpdateAsync(id, It.IsAny<TicketDTO>()), Times.Once);
    }

    [Test]
    public async Task Delete_DeveRetornarNoContent_QuandoDeletar()
    {
        var id = Guid.NewGuid();

        var service = new Mock<ITicketService>();
        service.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var controller = new TicketController(service.Object);

        var result = await controller.Delete(id);

        Assert.That(result, Is.TypeOf<NoContentResult>());

        service.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Test]
    public async Task Delete_DeveRetornarNotFound_QuandoNaoDeletar()
    {
        var id = Guid.NewGuid();

        var service = new Mock<ITicketService>();
        service.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var controller = new TicketController(service.Object);

        var result = await controller.Delete(id);

        Assert.That(result, Is.TypeOf<NotFoundResult>());

        service.Verify(s => s.DeleteAsync(id), Times.Once);
    }
}
