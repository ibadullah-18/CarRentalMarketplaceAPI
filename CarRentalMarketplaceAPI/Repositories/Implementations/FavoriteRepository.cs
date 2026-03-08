using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly AppDbContext _context;

    public FavoriteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(Guid userId)
    {
        return await _context.Favorites
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid userId, Guid carId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CarId == carId);

        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}