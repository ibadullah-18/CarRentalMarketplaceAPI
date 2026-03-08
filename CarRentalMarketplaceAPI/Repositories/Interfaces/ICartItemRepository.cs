using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(Guid cartId);

    Task AddAsync(CartItem item);

    Task RemoveAsync(Guid id);
}
