using NUnit.Framework;
using Moq;
using SistemaDeEventos.Services;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.DTOs.Rating;

namespace SistemaDeEventos.Tests;

[TestFixture]
public class RatingServiceTests
{
    private Mock<IRatingRepository> _mockRepository;
    private RatingService _ratingService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IRatingRepository>();
        _ratingService = new RatingService(_mockRepository.Object);
    }

    //create - sucessos

    [Test]
    public async Task CreateRating_DeveCriarRating_ComSucesso()
    {
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var score = 5;
        var comment = "Evento excelente";

        var result = await _ratingService
            .CreateRating(userId, eventId, score, comment);

        Assert.That(result.Score, Is.EqualTo(score));
        Assert.That(result.Comment, Is.EqualTo(comment));

        _mockRepository.Verify(r =>
            r.Create(It.Is<Rating>(x =>
                x.UserId == userId &&
                x.EventId == eventId &&
                x.Score == score &&
                x.Comment == comment
            )), Times.Once);
    }
    //create - falhas

    [Test]
    public void CreateRating_DeveLancarArgumentException_SeScoreMenorQue1()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _ratingService.CreateRating(
                Guid.NewGuid(),
                Guid.NewGuid(),
                0,
                "Teste"));
    }

    [Test]
    public void CreateRating_DeveLancarArgumentException_SeScoreMaiorQue5()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _ratingService.CreateRating(
                Guid.NewGuid(),
                Guid.NewGuid(),
                6,
                "Teste"));
    }

    [Test]
    public void CreateRating_DeveLancarArgumentException_SeUserIdInvalido()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _ratingService.CreateRating(
                Guid.Empty,
                Guid.NewGuid(),
                5,
                "Teste"));
    }

    [Test]
    public void CreateRating_DeveLancarArgumentException_SeEventIdInvalido()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _ratingService.CreateRating(
                Guid.NewGuid(),
                Guid.Empty,
                5,
                "Teste"));
    }

    [Test]
    public void CreateRating_DeveLancarArgumentException_SeCommentVazio()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _ratingService.CreateRating(
                Guid.NewGuid(),
                Guid.NewGuid(),
                5,
                ""));
    }

    //evento sucessos

    [Test]
    public async Task GetRatingsByEvent_DeveRetornarLista()
    {
        var eventId = Guid.NewGuid();

        var ratings = new List<Rating>
        {
            new Rating
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Score = 4,
                Comment = "Muito bom"
            },
            new Rating
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Score = 5,
                Comment = "Perfeito"
            }
        };

        _mockRepository
            .Setup(r => r.GetRatingsByEventId(eventId))
            .ReturnsAsync(ratings);

        var result = await _ratingService.GetRatingsByEvent(eventId);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Score, Is.EqualTo(4));
        Assert.That(result[1].Score, Is.EqualTo(5));
    }

    //evento falhas

    [Test]
    public void GetRatingsByEvent_DeveLancarArgumentException_SeEventIdInvalido()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _ratingService.GetRatingsByEvent(Guid.Empty));
    }
}

