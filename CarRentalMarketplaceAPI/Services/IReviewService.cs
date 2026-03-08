using CarRentalMarketplaceAPI.DTOs.Review;

namespace CarRentalMarketplaceAPI.Services;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetCarReviewsAsync(Guid carId);
    Task AddAsync(Guid userId, CreateReviewDto dto);
    Task DeleteAsync(Guid reviewId);
}
