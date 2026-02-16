using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Tests.Controllers;

[TestFixture]
public class EventsControllerTests
{
    private static EventDTO CreateDto(Guid? id = null)
    {
        return new EventDTO
        {
            Id = id ?? Guid.NewGuid(),
            NameEvents = "Show",
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Time = new TimeOnly(20, 0),
            Accessibility = true,
            LocationId = Guid.NewGuid()
        };
    }

    [Test]
    public async Task Get_SemFiltro_DeveRetornarOkComLista()
    {
        // Arrange
        var service = new Mock<IEventService>();
        service.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<EventDTO> { CreateDto(), CreateDto() });

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Get(null);

        // Assert
        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var data = ok!.Value as IEnumerable<EventDTO>;
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Count(), Is.EqualTo(2));

        // Verify
        service.Verify(s => s.GetAllAsync(), Times.Once);
        service.Verify(s => s.GetByAccessibilityAsync(It.IsAny<bool?>()), Times.Never);
    }

    [Test]
    public async Task Get_ComFiltroAccessibility_DeveRetornarOkComListaFiltrada()
    {
        // Arrange
        var service = new Mock<IEventService>();
        service.Setup(s => s.GetByAccessibilityAsync(true))
            .ReturnsAsync(new List<EventDTO> { CreateDto() });

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Get(true);

        // Assert
        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var data = ok!.Value as IEnumerable<EventDTO>;
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Count(), Is.EqualTo(1));

        // Verify
        service.Verify(s => s.GetByAccessibilityAsync(true), Times.Once);
        service.Verify(s => s.GetAllAsync(), Times.Never);
    }

    [Test]
    public async Task GetPorId_DeveRetornarOk_QuandoExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var service = new Mock<IEventService>();
        service.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(CreateDto(id));

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Get(id);

        // Assert
        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as EventDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));

        // Verify
        service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetPorId_DeveRetornarNotFound_QuandoNaoExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var service = new Mock<IEventService>();
        service.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((EventDTO?)null);

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Get(id);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());

        // Verify
        service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task Post_DeveRetornarCreatedAtAction()
    {
        // Arrange
        var created = CreateDto(Guid.NewGuid());

        var service = new Mock<IEventService>();
        service.Setup(s => s.CreateAsync(It.IsAny<EventDTO>()))
            .ReturnsAsync(created);

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Post(CreateDto());

        // Assert
        var createdAt = result.Result as CreatedAtActionResult;
        Assert.That(createdAt, Is.Not.Null);
        Assert.That(createdAt!.ActionName, Is.EqualTo("Get"));

        var dto = createdAt.Value as EventDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(created.Id));

        // Verify
        service.Verify(s => s.CreateAsync(It.IsAny<EventDTO>()), Times.Once);
    }

    [Test]
    public async Task Put_DeveRetornarOkComAtualizado()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updated = CreateDto(id);

        var service = new Mock<IEventService>();
        service.Setup(s => s.UpdateAsync(id, It.IsAny<EventDTO>()))
            .ReturnsAsync(updated);

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Put(id, CreateDto());

        // Assert
        var ok = result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as EventDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));

        // Verify
        service.Verify(s => s.UpdateAsync(id, It.IsAny<EventDTO>()), Times.Once);
    }

    [Test]
    public async Task Delete_DeveRetornarNoContent_QuandoDeletar()
    {
        // Arrange
        var id = Guid.NewGuid();

        var service = new Mock<IEventService>();
        service.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Delete(id);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());

        // Verify
        service.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Test]
    public async Task Delete_DeveRetornarNotFound_QuandoNaoDeletar()
    {
        // Arrange
        var id = Guid.NewGuid();

        var service = new Mock<IEventService>();
        service.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.Delete(id);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());

        // Verify
        service.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Test]
    public async Task GetRelatorio_DeveRetornarArquivoCsv()
    {
        // Arrange
        var service = new Mock<IEventService>();
        service.Setup(s => s.GetEventsReportCsvAsync())
            .ReturnsAsync("col1,col2\n1,2");

        var controller = new EventsController(service.Object);

        // Act
        var result = await controller.GetRelatorio();

        // Assert
        var file = result as FileContentResult;
        Assert.That(file, Is.Not.Null);
        Assert.That(file!.ContentType, Is.EqualTo("text/csv"));
        Assert.That(file.FileDownloadName, Does.Contain("eventos_"));

        // Verify
        service.Verify(s => s.GetEventsReportCsvAsync(), Times.Once);
    }
}
