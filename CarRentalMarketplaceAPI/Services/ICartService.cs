using CarRentalMarketplaceAPI.DTOs.Cart;

namespace CarRentalMarketplaceAPI.Services;

public interface ICartService
{
    Task<CartDto> GetCartAsync(Guid userId);
    Task AddItemAsync(Guid userId, AddCartItemDto dto);
    Task RemoveItemAsync(Guid itemId);
}
