using SistemaDeEventos.Models;

namespace SistemaDeEventos.Interfaces;

public interface IRatingRepository
{
    Task Create(Rating rating);

    Task<List<Rating>> GetRatingsByEventId(Guid eventId);

    Task<Rating?> GetById(Guid id);

    Task<List<Rating>> GetAll();

    Task<List<Rating>> GetRatingsByUserId(Guid userId);

    Task Update(Rating rating);

    Task Delete(Rating rating);
}

