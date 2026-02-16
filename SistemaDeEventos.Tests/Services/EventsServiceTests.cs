using Moq;
using NUnit.Framework;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.Services;

namespace SistemaDeEventos.Tests.Services;

[TestFixture]
public class EventsServiceTests
{
    private Event CreateModel(Guid? id = null)
    {
        return new Event
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

    private EventDTO CreateDto()
    {
        return new EventDTO
        {
            NameEvents = "Evento",
            Value = 200,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Time = new TimeOnly(19, 0),
            Accessibility = false,
            LocationId = Guid.NewGuid()
        };
    }

    [Test]
    public async Task GetAllAsync_DeveRetornarLista()
    {
        var repo = new Mock<IEventRepository>();

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Event>
            {
                CreateModel(),
                CreateModel()
            });

        var service = new EventService(repo.Object);

        var result = (await service.GetAllAsync()).ToList();

        Assert.That(result.Count, Is.EqualTo(2));

        repo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        var id = Guid.NewGuid();

        var repo = new Mock<IEventRepository>();
        repo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Event?)null);

        var service = new EventService(repo.Object);

        var result = await service.GetByIdAsync(id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_DeveGerarId()
    {
        var dto = CreateDto();

        var repo = new Mock<IEventRepository>();

        repo.Setup(r => r.AddAsync(It.IsAny<Event>()))
            .ReturnsAsync((Event e) => e);

        var service = new EventService(repo.Object);

        var result = await service.CreateAsync(dto);

        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));

        repo.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
    }
}

