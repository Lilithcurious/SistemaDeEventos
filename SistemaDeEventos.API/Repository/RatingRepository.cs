using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Models;

namespace SistemaDeEventos;

public class RatingRepository : IRatingRepository
{
    private readonly EventosContext _context;

    public RatingRepository(EventosContext context)
    {
        _context = context;
    }

    public async Task Create(Rating rating)
    {
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Rating>> GetRatingsByEventId(Guid eventId)
    {
        return await _context.Ratings
            .Where(r => r.EventId == eventId)
            .ToListAsync();
    }

    public async Task<List<Rating>> GetRatingsByUserId(Guid userId)
    {
        return await _context.Ratings
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Rating>> GetAll()
    {
        return await _context.Ratings
            .ToListAsync();
    }

    public async Task<Rating?> GetById(Guid id)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task Update(Rating rating)
    {
        _context.Ratings.Update(rating);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Rating rating)
    {
        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();
    }
}

