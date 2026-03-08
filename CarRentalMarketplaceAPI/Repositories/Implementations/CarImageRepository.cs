using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class CarImageRepository : ICarImageRepository
{
    private readonly AppDbContext _context;

    public CarImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarImage>> GetImagesByCarIdAsync(Guid carId)
    {
        return await _context.CarImages
            .Where(x => x.CarId == carId)
            .ToListAsync();
    }

    public async Task AddAsync(CarImage image)
    {
        await _context.CarImages.AddAsync(image);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var image = await _context.CarImages.FindAsync(id);

        if (image != null)
        {
            _context.CarImages.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}
