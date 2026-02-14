using SistemaDeEventos.DTOs.Rating;

namespace SistemaDeEventos;

public interface IRatingService
{
    Task<RatingResponseDTO> CreateRating(
        Guid userId,
        Guid eventId,
        int score,
        string comment);

    Task<List<RatingResponseDTO>> GetRatingsByEvent(Guid eventId);
}
