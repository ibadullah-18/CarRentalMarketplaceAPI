using CarRentalMarketplaceAPI.DTOs.Rental;

namespace CarRentalMarketplaceAPI.Services;

public interface IRentalService
{
    Task<IEnumerable<RentalDto>> GetUserRentalsAsync(Guid userId);
    Task CreateAsync(Guid userId, CreateRentalDto dto);
    Task CompleteAsync(Guid rentalId);
}
