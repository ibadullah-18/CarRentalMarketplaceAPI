using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface ICarRepository
{
    Task<IEnumerable<Car>> GetAllAsync();
    Task<Car> GetByIdAsync(Guid id);
    Task AddAsync(Car car);
    Task UpdateAsync(Car car);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Car>> GetCarsByOwnerAsync(Guid ownerId);
}
