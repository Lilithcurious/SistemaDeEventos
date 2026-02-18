using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTOs.Rating;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Tests.Controllers;

[TestFixture]
public class RatingControllerTests
{
    private static RatingCreateRequestDTO CreateRequest(Guid? userId = null, Guid? eventId = null)
        => new()
        {
            UserId = userId ?? Guid.NewGuid(),
            EventId = eventId ?? Guid.NewGuid(),
            Score = 5,
            Comment = "Top!"
        };

    private static RatingResponseDTO CreateResponse(Guid? id = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Score = 5,
            Comment = "Top!"
        };

    [Test]
    public async Task GetRatingsByEvent_QuandoListaVazia_DeveNotFound()
    {
        var eventId = Guid.NewGuid();
        var service = new Mock<IRatingService>();

        service.Setup(s => s.GetRatingsByEvent(eventId))
               .ReturnsAsync(new List<RatingResponseDTO>());

        var controller = new RatingController(service.Object);

        var result = await controller.GetRatingsByEvent(eventId);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetRatingsByEvent_QuandoNull_DeveNotFound()
    {
        var eventId = Guid.NewGuid();
        var service = new Mock<IRatingService>();

        service.Setup(s => s.GetRatingsByEvent(eventId))
               .ReturnsAsync((List<RatingResponseDTO>?)null);

        var controller = new RatingController(service.Object);

        var result = await controller.GetRatingsByEvent(eventId);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetRatingsByEvent_QuandoTemItens_DeveOk()
    {
        var eventId = Guid.NewGuid();
        var service = new Mock<IRatingService>();

        service.Setup(s => s.GetRatingsByEvent(eventId))
               .ReturnsAsync(new List<RatingResponseDTO> { CreateResponse(), CreateResponse() });

        var controller = new RatingController(service.Object);

        var result = await controller.GetRatingsByEvent(eventId);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var list = ok!.Value as List<RatingResponseDTO>;
        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task CreateRating_ModelStateInvalido_DeveBadRequest()
    {
        var service = new Mock<IRatingService>();
        var controller = new RatingController(service.Object);

        controller.ModelState.AddModelError("Score", "Obrigatório");

        var result = await controller.CreateRating(CreateRequest());

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        service.Verify(s => s.CreateRating(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task CreateRating_DeveCreatedAtAction()
    {
        var service = new Mock<IRatingService>();
        var request = CreateRequest();

        var created = new RatingResponseDTO
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            EventId = request.EventId,
            Score = request.Score,
            Comment = request.Comment
        };

        service.Setup(s => s.CreateRating(request.UserId, request.EventId, request.Score, request.Comment))
               .ReturnsAsync(created);

        var controller = new RatingController(service.Object);

        var result = await controller.CreateRating(request);

        var createdAt = result.Result as CreatedAtActionResult;
        Assert.That(createdAt, Is.Not.Null);
        Assert.That(createdAt!.ActionName, Is.EqualTo(nameof(RatingController.GetRatingsByEvent)));

        var dto = createdAt.Value as RatingResponseDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.EventId, Is.EqualTo(request.EventId));
    }

    [Test]
    public async Task CreateRating_ArgumentException_DeveBadRequest()
    {
        var service = new Mock<IRatingService>();
        var request = CreateRequest();

        service.Setup(s => s.CreateRating(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>()))
               .ThrowsAsync(new ArgumentException("Score inválido"));

        var controller = new RatingController(service.Object);

        var result = await controller.CreateRating(request);

        var bad = result.Result as BadRequestObjectResult;
        Assert.That(bad, Is.Not.Null);
        Assert.That(bad!.Value, Is.EqualTo("Score inválido"));
    }
}
