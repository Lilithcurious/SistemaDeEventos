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
    private Mock<ITicketRepository> _repoMock = null!;
    private TicketService _service = null!;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<ITicketRepository>(MockBehavior.Strict);
        _service = new TicketService(_repoMock.Object);
    }

    private static Ticket CreateTicketModel(Guid? id = null)
        => new Ticket
        {
            Id = id ?? Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Quantity = 2,
            Value = 150.50m,
            Date = new DateOnly(2026, 2, 16),
            Time = new TimeOnly(18, 30),
            TicketType = "VIP",
            Accessibility = true
        };

    private static TicketDTO CreateTicketDto(Guid? id = null)
        => new TicketDTO
        {
            Id = id ?? Guid.Empty, // no CreateAsync o service gera Guid
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Quantity = 1,
            Value = 99.90m,
            Date = new DateOnly(2026, 2, 16),
            Time = new TimeOnly(20, 0),
            TicketType = "Pista",
            Accessibility = false
        };

    // -----------------------
    // GetByIdAsync
    // -----------------------

    [Test]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoEncontrar()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Ticket?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.That(result, Is.Null);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetByIdAsync_DeveRetornarDTO_QuandoEncontrar()
    {
        // Arrange
        var model = CreateTicketModel();
        _repoMock
            .Setup(r => r.GetByIdAsync(model.Id))
            .ReturnsAsync(model);

        // Act
        var result = await _service.GetByIdAsync(model.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(model.Id));
        Assert.That(result.OrderId, Is.EqualTo(model.OrderId));
        Assert.That(result.UserId, Is.EqualTo(model.UserId));
        Assert.That(result.EventId, Is.EqualTo(model.EventId));
        Assert.That(result.Quantity, Is.EqualTo(model.Quantity));
        Assert.That(result.Value, Is.EqualTo(model.Value));
        Assert.That(result.Date, Is.EqualTo(model.Date));
        Assert.That(result.Time, Is.EqualTo(model.Time));
        Assert.That(result.TicketType, Is.EqualTo(model.TicketType));
        Assert.That(result.Accessibility, Is.EqualTo(model.Accessibility));

        _repoMock.Verify(r => r.GetByIdAsync(model.Id), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    // -----------------------
    // GetAllAsync
    // -----------------------

    [Test]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoRepositorioVazio()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Ticket>());

        // Act
        var result = (await _service.GetAllAsync()).ToList();

        // Assert
        Assert.That(result, Is.Empty);
        _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetAllAsync_DeveRetornarListaDTO_QuandoExistiremTickets()
    {
        // Arrange
        var t1 = CreateTicketModel();
        var t2 = CreateTicketModel();

        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Ticket> { t1, t2 });

        // Act
        var result = (await _service.GetAllAsync()).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(t1.Id));
        Assert.That(result[1].Id, Is.EqualTo(t2.Id));

        _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    // -----------------------
    // Filters
    // -----------------------

    [Test]
    public async Task GetByUserIdAsync_DeveRetornarListaDTO()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tickets = new List<Ticket>
        {
            CreateTicketModel(),
            CreateTicketModel()
        };

        _repoMock
            .Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(tickets);

        // Act
        var result = (await _service.GetByUserIdAsync(userId)).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        _repoMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetByOrderIdAsync_DeveRetornarListaDTO()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByOrderIdAsync(orderId))
            .ReturnsAsync(new List<Ticket> { CreateTicketModel() });

        // Act
        var result = (await _service.GetByOrderIdAsync(orderId)).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        _repoMock.Verify(r => r.GetByOrderIdAsync(orderId), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public async Task GetByAccessibilityAsync_DeveRetornarListaDTO(bool? accessibility)
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByAccessibilityAsync(accessibility))
            .ReturnsAsync(new List<Ticket> { CreateTicketModel() });

        // Act
        var result = (await _service.GetByAccessibilityAsync(accessibility)).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        _repoMock.Verify(r => r.GetByAccessibilityAsync(accessibility), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    // -----------------------
    // CreateAsync
    // -----------------------

    [Test]
    public async Task CreateAsync_DeveGerarNovoGuidEChamarAddAsync()
    {
        // Arrange
        var dto = CreateTicketDto();

        Ticket? captured = null;

        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<Ticket>()))
            .Callback<Ticket>(t => captured = t)
            .ReturnsAsync((Ticket t) => t); // devolve o mesmo objeto "criado"

        // Act
        var created = await _service.CreateAsync(dto);

        // Assert
        Assert.That(captured, Is.Not.Null);
        Assert.That(captured!.Id, Is.Not.EqualTo(Guid.Empty)); // service deve gerar
        Assert.That(created.Id, Is.EqualTo(captured.Id));

        // garante mapeamento do DTO -> Model
        Assert.That(captured.OrderId, Is.EqualTo(dto.OrderId));
        Assert.That(captured.UserId, Is.EqualTo(dto.UserId));
        Assert.That(captured.EventId, Is.EqualTo(dto.EventId));
        Assert.That(captured.Quantity, Is.EqualTo(dto.Quantity));
        Assert.That(captured.Value, Is.EqualTo(dto.Value));
        Assert.That(captured.Date, Is.EqualTo(dto.Date));
        Assert.That(captured.Time, Is.EqualTo(dto.Time));
        Assert.That(captured.TicketType, Is.EqualTo(dto.TicketType));
        Assert.That(captured.Accessibility, Is.EqualTo(dto.Accessibility));

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Ticket>()), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    // -----------------------
    // UpdateAsync
    // -----------------------

    [Test]
    public void UpdateAsync_DeveLancarExcecao_QuandoNaoEncontrar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = CreateTicketDto();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Ticket?)null);

        // Act + Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.UpdateAsync(id, dto));
        Assert.That(ex!.Message, Does.Contain("não encontrado"));

        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task UpdateAsync_DeveAtualizarCamposEChamarUpdateAsync()
    {
        // Arrange
        var existing = CreateTicketModel();
        var id = existing.Id;

        var dto = new TicketDTO
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Quantity = 10,
            Value = 777.77m,
            Date = new DateOnly(2026, 12, 25),
            Time = new TimeOnly(9, 15),
            TicketType = "Meia",
            Accessibility = true
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _repoMock
            .Setup(r => r.UpdateAsync(It.IsAny<Ticket>()))
            .ReturnsAsync((Ticket t) => t);

        // Act
        var updated = await _service.UpdateAsync(id, dto);

        // Assert
        Assert.That(updated.Id, Is.EqualTo(id));
        Assert.That(updated.OrderId, Is.EqualTo(dto.OrderId));
        Assert.That(updated.UserId, Is.EqualTo(dto.UserId));
        Assert.That(updated.EventId, Is.EqualTo(existing.EventId)); 
        // ⚠️ no seu service UpdateAsync NÃO altera EventId.
        // (se quiser que altere, é no código principal. Como você não quer mexer, testamos como está.)

        Assert.That(updated.Quantity, Is.EqualTo(dto.Quantity));
        Assert.That(updated.Value, Is.EqualTo(dto.Value));
        Assert.That(updated.Date, Is.EqualTo(dto.Date));
        Assert.That(updated.Time, Is.EqualTo(dto.Time));
        Assert.That(updated.TicketType, Is.EqualTo(dto.TicketType));
        Assert.That(updated.Accessibility, Is.EqualTo(dto.Accessibility));

        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Ticket>()), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }

    // -----------------------
    // DeleteAsync
    // -----------------------

    [TestCase(true)]
    [TestCase(false)]
    public async Task DeleteAsync_DeveRetornarResultadoDoRepositorio(bool repoResult)
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(repoResult);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.That(result, Is.EqualTo(repoResult));
        _repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
        _repoMock.VerifyNoOtherCalls();
    }
}
