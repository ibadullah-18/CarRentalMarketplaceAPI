using Microsoft.AspNetCore.Http;
using CarRentalMarketplaceAPI.DTOs.Car;

namespace CarRentalMarketplaceAPI.Services;

public interface ICarService
{
    Task<IEnumerable<CarListDto>> GetAllAsync();
    Task<CarDetailDto> GetByIdAsync(Guid id);
    Task DeleteImageAsync(Guid imageId, Guid userId);
    Task SetMainImageAsync(Guid imageId, Guid userId);
    Task<CarDto> CreateAsync(Guid ownerId, CreateCarDto dto);
    Task UpdateAsync(Guid id, Guid userId, UpdateCarDto dto);
    Task DeleteAsync(Guid id, Guid userId);
    Task<IEnumerable<OwnerCarsDto>> GetCarsByOwnerAsync(Guid ownerId);
    Task ActivateAsync(Guid id, Guid userId);
    Task AddImageAsync(Guid carId, IFormFile file, bool isMain, Guid userId);
    Task<IEnumerable<CarListDto>> GetFilteredCarsAsync(CarQueryDto query);
}