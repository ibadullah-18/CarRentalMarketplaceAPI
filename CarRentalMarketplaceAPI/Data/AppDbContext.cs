using CarRentalMarketplaceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarImage> CarImages { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
}
