using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface IRentalRepository
{
    Task<Rental> GetByIdAsync(Guid id);
    Task<IEnumerable<Rental>> GetUserRentalsAsync(Guid userId);
    Task<IEnumerable<Rental>> GetOwnerRentalsAsync(Guid ownerId);
    Task AddAsync(Rental rental);
    Task UpdateAsync(Rental rental);
}
