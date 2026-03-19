using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Rental> GetByIdAsync(Guid id)
    {
        return await _context.Rentals.FindAsync(id);
    }

    public async Task<IEnumerable<Rental>> GetUserRentalsAsync(Guid userId)
    {
        return await _context.Rentals
            .Where(x => x.RenterId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetOwnerRentalsAsync(Guid ownerId)
    {
        return await _context.Rentals
            .Where(r => _context.Cars.Any(c => c.Id == r.CarId && c.OwnerId == ownerId))
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task AddAsync(Rental rental)
    {
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }
}