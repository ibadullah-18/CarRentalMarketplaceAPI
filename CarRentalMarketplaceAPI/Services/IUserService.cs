using CarRentalMarketplaceAPI.DTOs.User;

namespace CarRentalMarketplaceAPI.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdateUserDto dto);
}
