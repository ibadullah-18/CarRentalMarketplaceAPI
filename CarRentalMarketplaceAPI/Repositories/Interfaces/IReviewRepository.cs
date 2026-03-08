using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetCarReviewsAsync(Guid carId);

    Task AddAsync(Review review);

    Task DeleteAsync(Guid id);
}
