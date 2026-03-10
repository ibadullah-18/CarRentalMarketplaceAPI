
using Microsoft.AspNetCore.Http;
using CarRentalMarketplaceAPI.DTOs.Car;

namespace CarRentalMarketplaceAPI.Services;

public interface ICarService
{
    Task<IEnumerable<CarListDto>> GetAllAsync();
    Task<CarDetailDto> GetByIdAsync(Guid id);
    Task CreateAsync(Guid ownerId, CreateCarDto dto);
    Task UpdateAsync(Guid id, Guid userId, UpdateCarDto dto);
    Task DeleteAsync(Guid id, Guid userId);
    Task<IEnumerable<CarListDto>> GetCarsByOwnerAsync(Guid ownerId);
    Task ActivateAsync(Guid id, Guid userId);
    Task AddImageAsync(Guid carId, IFormFile file, bool isMain);
}

