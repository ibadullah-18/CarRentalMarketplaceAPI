using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface ICarImageRepository
{
    Task<IEnumerable<CarImage>> GetImagesByCarIdAsync(Guid carId);
    Task AddAsync (CarImage carImage);
    Task DeleteAsync (Guid id);
}
