using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetCarReviewsAsync(Guid carId)
    {
        return await _context.Reviews
            .Where(x => x.CarId == carId)
            .ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}