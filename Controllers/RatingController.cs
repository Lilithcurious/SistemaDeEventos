using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.DTOs.Rating;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
        }

        [HttpGet("event/{eventId:guid}")]
        public async Task<ActionResult<List<RatingResponseDTO>>> GetRatingsByEvent(Guid eventId)
        {
            var ratings = await _ratingService.GetRatingsByEvent(eventId);

            if (ratings == null || !ratings.Any())
                return NotFound();

            return Ok(ratings);
        }

        [HttpPost]
        public async Task<ActionResult<RatingResponseDTO>> CreateRating(
            [FromBody] RatingCreateRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var rating = await _ratingService.CreateRating(
                    request.UserId,
                    request.EventId,
                    request.Score,
                    request.Comment);

                return CreatedAtAction(
                    nameof(GetRatingsByEvent),
                    new { eventId = request.EventId },
                    rating);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
