using Moq;
using NUnit.Framework;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.Services;

namespace SistemaDeEventos.Tests.Services;

[TestFixture]
public class LocationsServiceTests
{
    private Location CreateModel(Guid? id = null)
    {
        return new Location
        {
            Id = id ?? Guid.NewGuid(),
            Address = "Rua Teste",
            Capacity = 100
        };
    }

    private LocationDTO CreateDto()
    {
        return new LocationDTO
        {
            Address = "Rua Nova",
            Capacity = 500
        };
    }

    [Test]
    public async Task GetAllAsync_DeveRetornarLista()
    {
        var repo = new Mock<ILocationRepository>();

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Location>
            {
                CreateModel(),
                CreateModel()
            });

        var service = new LocationService(repo.Object);

        var result = (await service.GetAllAsync()).ToList();

        Assert.That(result.Count, Is.EqualTo(2));

        repo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task CreateAsync_DeveGerarId()
    {
        var dto = CreateDto();

        var repo = new Mock<ILocationRepository>();

        repo.Setup(r => r.AddAsync(It.IsAny<Location>()))
            .ReturnsAsync((Location l) => l);

        var service = new LocationService(repo.Object);

        var result = await service.CreateAsync(dto);

        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));

        repo.Verify(r => r.AddAsync(It.IsAny<Location>()), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        var id = Guid.NewGuid();

        var repo = new Mock<ILocationRepository>();
        repo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Location?)null);

        var service = new LocationService(repo.Object);

        var result = await service.GetByIdAsync(id);

        Assert.That(result, Is.Null);
    }
}
