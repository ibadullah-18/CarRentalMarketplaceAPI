using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext _context;

    public CartItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(Guid cartId)
    {
        return await _context.CartItems
            .Where(x => x.CartId == cartId)
            .ToListAsync();
    }

    public async Task AddAsync(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var item = await _context.CartItems.FindAsync(id);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CartItem> GetByCartIdAndCarIdAsync(Guid cartId, Guid carId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cartId && x.CarId == carId);
    }
}