using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface IFavoriteRepository
{
    Task<IEnumerable<Favorite>> GetUserFavoritesAsync(Guid userId);

    Task AddAsync(Favorite favorite);

    Task RemoveAsync(Guid userId, Guid carId);
}
