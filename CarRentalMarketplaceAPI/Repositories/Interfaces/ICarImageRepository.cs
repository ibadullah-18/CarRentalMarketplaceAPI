using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface ICarImageRepository
{
    Task<IEnumerable<CarImage>> GetImagesByCarIdAsync(Guid carId);
    Task<CarImage> GetMainImageByCarIdAsync(Guid carId);
    Task<CarImage> GetByIdAsync(Guid id);
    Task UpdateAsync(CarImage image);
    Task AddAsync (CarImage carImage);
    Task DeleteAsync (Guid id);
}
