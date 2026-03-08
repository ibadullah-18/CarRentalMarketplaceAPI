using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartByUserIdAsync(Guid userId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cart = await _context.Carts.FindAsync(id);

        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }
}