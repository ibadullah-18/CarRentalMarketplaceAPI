using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(Guid cartId);
    Task<CartItem> GetByCartIdAndCarIdAsync(Guid cartId, Guid carId);
    Task AddAsync(CartItem item);
    Task RemoveAsync(Guid id);
}
