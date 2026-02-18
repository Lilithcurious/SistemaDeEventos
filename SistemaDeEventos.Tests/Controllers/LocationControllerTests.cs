using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Tests.Controllers;

[TestFixture]
public class LocationsControllerTests
{
    private EventosContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<EventosContext>()
            .UseInMemoryDatabase(databaseName: $"db_{Guid.NewGuid()}")
            .Options;

        return new EventosContext(options);
    }

    [Test]
    public async Task Get_DeveRetornarLista()
    {
        // Arrange
        using var db = CreateDbContext();
        db.Locations.Add(new Location { Id = Guid.NewGuid(), Address = "Rua A", Capacity = 10 });
        db.Locations.Add(new Location { Id = Guid.NewGuid(), Address = "Rua B", Capacity = 20 });
        await db.SaveChangesAsync();

        var controller = new LocationController(db);

        // Act
        var result = await controller.Get();

        // Assert
        var okList = result.Value;
        Assert.That(okList, Is.Not.Null);
        Assert.That(okList!.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetPorId_DeveRetornarLocation_QuandoExistir()
    {
        // Arrange
        using var db = CreateDbContext();
        var id = Guid.NewGuid();
        db.Locations.Add(new Location { Id = id, Address = "Rua Teste", Capacity = 100 });
        await db.SaveChangesAsync();

        var controller = new LocationController(db);

        // Act
        var result = await controller.Get(id);

        // Assert
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetPorId_DeveRetornarNotFound_QuandoNaoExistir()
    {
        // Arrange
        using var db = CreateDbContext();
        var controller = new LocationController(db);

        // Act
        var result = await controller.Get(Guid.NewGuid());

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Post_DeveCriarELocalizarComGet()
    {
        // Arrange
        using var db = CreateDbContext();
        var controller = new LocationController(db);

        var location = new Location
        {
            Id = Guid.NewGuid(),
            Address = "Av Brasil",
            Capacity = 500
        };

        // Act
        var postResult = await controller.Post(location);

        // Assert
        var created = postResult.Result as CreatedAtActionResult;
        Assert.That(created, Is.Not.Null);

        var createdValue = created!.Value as Location;
        Assert.That(createdValue, Is.Not.Null);
        Assert.That(createdValue!.Address, Is.EqualTo("Av Brasil"));

        // Confere no banco
        var saved = await db.Locations.FindAsync(location.Id);
        Assert.That(saved, Is.Not.Null);
    }

    [Test]
    public async Task Delete_DeveRetornarNoContent_QuandoExistir()
    {
        // Arrange
        using var db = CreateDbContext();
        var id = Guid.NewGuid();
        db.Locations.Add(new Location { Id = id, Address = "Rua X", Capacity = 1 });
        await db.SaveChangesAsync();

        var controller = new LocationController(db);

        // Act
        var result = await controller.Delete(id);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
        Assert.That(await db.Locations.FindAsync(id), Is.Null);
    }

    [Test]
    public async Task Delete_DeveRetornarNotFound_QuandoNaoExistir()
    {
        // Arrange
        using var db = CreateDbContext();
        var controller = new LocationController(db);

        // Act
        var result = await controller.Delete(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}
