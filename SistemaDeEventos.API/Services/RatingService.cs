using SistemaDeEventos.DTOs.Rating;
using SistemaDeEventos.Models;

namespace SistemaDeEventos;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<RatingResponseDTO> CreateRating(
        Guid userId, 
        Guid eventId, 
        int score, 
        string comment)
    {
        if (score < 1 || score > 5)
            throw new ArgumentException("A nota deve estar entre 1 e 5.");

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
        var ratings = await _ratingRepository.GetRatingsByEventId(eventId);

        return ratings
            .Select(r => new RatingResponseDTO
            {
                Id = r.Id,
                Score = r.Score,
                Comment = r.Comment
            })
            .ToList();
    }
}
