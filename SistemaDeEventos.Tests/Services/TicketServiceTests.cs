using Moq;
using NUnit.Framework;
using SistemaDeEventos.DTO;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.Services;

namespace SistemaDeEventos.Tests.Services;

[TestFixture]
public class TicketServiceTests
{
    private static Ticket CreateTicketModel(Guid? id = null)
    {
        return new Ticket
        {
            Id = id ?? Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Quantity = 2,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Time = new TimeOnly(20, 0),
            TicketType = "VIP",
            Accessibility = false
        };
    }

    private static TicketDTO CreateTicketDto()
    {
        return new TicketDTO
        {
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
    public async Task GetAllAsync_DeveRetornarLista_QuandoExistiremTickets()
    {
        // Arrange
        var repoMock = new Mock<ITicketRepository>();
        repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Ticket> { CreateTicketModel(), CreateTicketModel() });

        var service = new TicketService(repoMock.Object);

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));

        // Verify
        repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoExistiremTickets()
    {
        // Arrange
        var repoMock = new Mock<ITicketRepository>();
        repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Ticket>());

        var service = new TicketService(repoMock.Object);

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));

        // Verify
        repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_DeveRetornarDTO_QuandoTicketExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ticket = CreateTicketModel(id);

        var repoMock = new Mock<ITicketRepository>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(ticket);

        var service = new TicketService(repoMock.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(id));

        // Verify
        repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoTicketNaoExistir()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repoMock = new Mock<ITicketRepository>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Ticket?)null);

        var service = new TicketService(repoMock.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.That(result, Is.Null);

        // Verify
        repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task CreateAsync_DeveGerarIdEChamarAddAsync()
    {
        // Arrange
        var dto = CreateTicketDto();

        var repoMock = new Mock<ITicketRepository>();

        // Captura o Ticket que o service mandar pro repo
        Ticket? ticketRecebido = null;

        repoMock
            .Setup(r => r.AddAsync(It.IsAny<Ticket>()))
            .Callback<Ticket>(t => ticketRecebido = t)
            .ReturnsAsync((Ticket t) => t); // devolve o mesmo objeto “criado”

        var service = new TicketService(repoMock.Object);

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(ticketRecebido, Is.Not.Null);
        Assert.That(ticketRecebido!.Id, Is.Not.EqualTo(Guid.Empty));

        // Verify
        repoMock.Verify(r => r.AddAsync(It.IsAny<Ticket>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_DeveAtualizarEChamarUpdateAsync_QuandoTicketExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ticketExistente = CreateTicketModel(id);
        var dtoAtualizacao = CreateTicketDto();
        dtoAtualizacao.OrderId = Guid.NewGuid();
        dtoAtualizacao.UserId = Guid.NewGuid();

        var repoMock = new Mock<ITicketRepository>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(ticketExistente);
        repoMock.Setup(r => r.UpdateAsync(It.IsAny<Ticket>()))
            .ReturnsAsync((Ticket t) => t);

        var service = new TicketService(repoMock.Object);

        // Act
        var result = await service.UpdateAsync(id, dtoAtualizacao);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.OrderId, Is.EqualTo(dtoAtualizacao.OrderId));
        Assert.That(result.UserId, Is.EqualTo(dtoAtualizacao.UserId));

        // Verify
        repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Ticket>()), Times.Once);
    }

    [Test]
    public void UpdateAsync_DeveLancarExcecao_QuandoTicketNaoExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = CreateTicketDto();

        var repoMock = new Mock<ITicketRepository>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Ticket?)null);

        var service = new TicketService(repoMock.Object);

        // Act + Assert
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.UpdateAsync(id, dto);
        });

        // Verify
        repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Ticket>()), Times.Never);
    }
}
