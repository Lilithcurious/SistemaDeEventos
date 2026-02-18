using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTOs.Rating;
using SistemaDeEventos.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDeEventos.Tests.Controllers
{
    public class RatingControllerTests
    {
        private Mock<IRatingService> _mockRatingService;
        private RatingController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRatingService = new Mock<IRatingService>();
            _controller = new RatingController(_mockRatingService.Object);
        }

        [Test]
        public async Task GetRatingsByEvent_WithRatings_ReturnsOk()
        {
            var eventId = Guid.NewGuid();

            var ratings = new List<RatingResponseDTO>
            {
                new RatingResponseDTO
                {
                    Id = Guid.NewGuid(),
                    Score = 5,
                    Comment = "Excelente!"
                }
            };

            _mockRatingService
                .Setup(s => s.GetRatingsByEvent(eventId))
                .ReturnsAsync(ratings);

            var result = await _controller.GetRatingsByEvent(eventId);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;
            var returnedRatings = (List<RatingResponseDTO>)okResult.Value!;

            Assert.That(returnedRatings.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetRatingsByEvent_NoRatings_ReturnsNotFound()
        {
            var eventId = Guid.NewGuid();

            _mockRatingService
                .Setup(s => s.GetRatingsByEvent(eventId))
                .ReturnsAsync(new List<RatingResponseDTO>());

            var result = await _controller.GetRatingsByEvent(eventId);

            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateRating_ValidRequest_ReturnsCreatedAtAction()
        {
            var request = new RatingCreateRequestDTO
            {
                UserId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
                Score = 4,
                Comment = "Muito bom!"
            };

            var response = new RatingResponseDTO
            {
                Id = Guid.NewGuid(),
                Score = request.Score,
                Comment = request.Comment
            };

            _mockRatingService
                .Setup(s => s.CreateRating(
                    request.UserId,
                    request.EventId,
                    request.Score,
                    request.Comment))
                .ReturnsAsync(response);

            var result = await _controller.CreateRating(request);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

            var createdResult = (CreatedAtActionResult)result.Result!;
            var returnedRating = (RatingResponseDTO)createdResult.Value!;

            Assert.That(createdResult.ActionName, Is.EqualTo("GetRatingsByEvent"));
            Assert.That(returnedRating.Score, Is.EqualTo(request.Score));
        }

        [Test]
        public async Task CreateRating_ServiceThrowsArgumentException_ReturnsBadRequest()
        {
            var request = new RatingCreateRequestDTO
            {
                UserId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
                Score = 0,
                Comment = "Inválido"
            };

            _mockRatingService
                .Setup(s => s.CreateRating(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("A nota deve estar entre 1 e 5."));

            var result = await _controller.CreateRating(request);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

            var badRequestResult = (BadRequestObjectResult)result.Result!;
            Assert.That(badRequestResult.Value, Is.EqualTo("A nota deve estar entre 1 e 5."));
        }
    }
}
