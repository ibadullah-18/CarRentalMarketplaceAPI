using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart> GetCartByUserIdAsync(Guid userId);

    Task AddAsync(Cart cart);

    Task UpdateAsync(Cart cart);

    Task DeleteAsync(Guid id);
}
