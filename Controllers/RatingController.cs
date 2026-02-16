using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.DTOs.Rating;
using SistemaDeEventos.Interfaces;
using System.Linq;


namespace SistemaDeEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

       [HttpGet("event/{eventId}")]
        public async Task<ActionResult<List<RatingResponseDTO>>> GetRatingsByEvent(Guid eventId)
        {
            var ratings = await _ratingService.GetRatingsByEvent(eventId);

            if (ratings == null || !ratings.Any())
            return NotFound();

            return Ok(ratings);
    }

        [HttpPost]
        public async Task<ActionResult<RatingResponseDTO>> CreateRating(
        RatingCreateRequestDTO request)
        {
        var rating = await _ratingService.CreateRating(
        request.UserId,
        request.EventId,
        request.Score,
        request.Comment);

        return CreatedAtAction(
        nameof(GetRatingsByEvent),
        new { eventId = rating.EventId },
        rating);
        }
    }
}

