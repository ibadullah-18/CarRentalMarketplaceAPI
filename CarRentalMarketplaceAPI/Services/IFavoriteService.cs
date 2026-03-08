using CarRentalMarketplaceAPI.DTOs.Favorite;

namespace CarRentalMarketplaceAPI.Services;

public interface IFavoriteService
{
    Task<IEnumerable<FavoriteDto>> GetUserFavoritesAsync(Guid userId);
    Task AddAsync(Guid userId, AddFavoriteDto dto);
    Task RemoveAsync(Guid userId, Guid carId);
}
