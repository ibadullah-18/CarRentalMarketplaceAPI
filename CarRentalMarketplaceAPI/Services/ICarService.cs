using CarRentalMarketplaceAPI.DTOs.Car;

namespace CarRentalMarketplaceAPI.Services;

public interface ICarService
{
    Task<IEnumerable<CarListDto>> GetAllAsync();
    Task<CarDetailDto> GetByIdAsync(Guid id);
    Task CreateAsync(Guid ownerId, CreateCarDto dto);
    Task UpdateAsync(Guid id, UpdateCarDto dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<CarListDto>> GetCarsByOwnerAsync(Guid ownerId);
}

