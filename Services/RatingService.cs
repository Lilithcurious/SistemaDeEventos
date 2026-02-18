using SistemaDeEventos.DTOs.Rating;
using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository
            ?? throw new ArgumentNullException(nameof(ratingRepository));
    }

    public async Task<RatingResponseDTO> CreateRating(
        Guid userId,
        Guid eventId,
        int score,
        string comment)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        if (eventId == Guid.Empty)
            throw new ArgumentException("Invalid event ID");

        if (score < 1 || score > 5)
            throw new ArgumentException("Score must be between 1 and 5");

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required");

        var rating = new Rating
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventId = eventId,
            Score = score,
            Comment = comment
        };

        await _ratingRepository.Create(rating);

        return new RatingResponseDTO
        {
            Id = rating.Id,
            Score = rating.Score,
            Comment = rating.Comment
        };
    }

    public async Task<List<RatingResponseDTO>> GetRatingsByEvent(Guid eventId)
    {
        if (eventId == Guid.Empty)
            throw new ArgumentException("Invalid event ID");

        var ratings = await _ratingRepository.GetRatingsByEventId(eventId);

        if (ratings == null)
            return new List<RatingResponseDTO>();

        return ratings
            .Select(r => new RatingResponseDTO
            {
                Id = r.Id,
                Score = r.Score,
                Comment = r.Comment ?? string.Empty
            })
            .ToList();
    }
}



