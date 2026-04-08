using CarRentalMarketplaceAPI.Data;
using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Enums;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.Repositories.Implementations;

public class CarRepository : ICarRepository
{
    private readonly AppDbContext _context;

    public CarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        return await _context.Cars
            .Where(x => x.Status == CarStatus.Available)
            .ToListAsync();
    }

    public async Task<Car> GetByIdAsync(Guid id)
    {
        return await _context.Cars.FindAsync(id);
    }

    public async Task AddAsync(Car car)
    {
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Car car)
    {
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var car = await _context.Cars.FindAsync(id);

        if (car != null)
        {
            car.Status = CarStatus.Passive;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }
    }

    // sadece oz masinlarim
    public async Task<IEnumerable<Car>> GetCarsByOwnerAsync(Guid ownerId)
    {
        return await _context.Cars
            .Where(x => x.OwnerId == ownerId)
            .ToListAsync();
    }

    // bashqa userin profiline baxanda sadece active/available olanlar
    public async Task<IEnumerable<Car>> GetPublicCarsByOwnerIdAsync(Guid ownerId)
    {
        return await _context.Cars
            .Where(x => x.OwnerId == ownerId && x.Status == CarStatus.Available)
            .ToListAsync();
    }

    public async Task<IEnumerable<Car>> GetFilteredCarsAsync(CarQueryDto query)
    {
        var carsQuery = _context.Cars
            .Where(x => x.Status == CarStatus.Available)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            carsQuery = carsQuery.Where(x =>
                x.Brand.ToLower().Contains(search) ||
                x.Model.ToLower().Contains(search) ||
                x.Description.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Brand))
        {
            var brand = query.Brand.ToLower();
            carsQuery = carsQuery.Where(x => x.Brand.ToLower() == brand);
        }

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            var location = query.Location.ToLower();
            carsQuery = carsQuery.Where(x => x.Location.ToLower().Contains(location));
        }

        if (!string.IsNullOrWhiteSpace(query.BodyType))
        {
            if (Enum.TryParse<BodyType>(query.BodyType, true, out var parsedBodyType))
            {
                carsQuery = carsQuery.Where(x => x.BodyType == parsedBodyType);
            }
        }

        if (query.MinPrice.HasValue)
        {
            carsQuery = carsQuery.Where(x => x.PricePerDay >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            carsQuery = carsQuery.Where(x => x.PricePerDay <= query.MaxPrice.Value);
        }

        carsQuery = query.SortBy?.ToLower() switch
        {
            "priceasc" => carsQuery.OrderBy(x => x.PricePerDay),
            "pricedesc" => carsQuery.OrderByDescending(x => x.PricePerDay),
            "newest" => carsQuery.OrderByDescending(x => x.CreatedDate),
            "oldest" => carsQuery.OrderBy(x => x.CreatedDate),
            _ => carsQuery.OrderByDescending(x => x.CreatedDate)
        };

        return await carsQuery.ToListAsync();
    }

    public async Task HardDeleteAsync(Guid id)
    {
        var car = await _context.Cars.FindAsync(id);

        if (car == null)
            return;

        if (car.Status != CarStatus.Available && car.Status != CarStatus.Passive)
            throw new BadRequestException("Only available or passive cars can be permanently deleted.");

        var cartItems = await _context.CartItems
            .Where(x => x.CarId == id)
            .ToListAsync();

        var rentals = await _context.Rentals
            .Where(x => x.CarId == id)
            .ToListAsync();

        var favorites = await _context.Favorites
            .Where(x => x.CarId == id)
            .ToListAsync();

        var reviews = await _context.Reviews
            .Where(x => x.CarId == id)
            .ToListAsync();

        var images = await _context.CarImages
            .Where(x => x.CarId == id)
            .ToListAsync();

        if (cartItems.Any())
            _context.CartItems.RemoveRange(cartItems);

        if (rentals.Any())
            _context.Rentals.RemoveRange(rentals);

        if (favorites.Any())
            _context.Favorites.RemoveRange(favorites);

        if (reviews.Any())
            _context.Reviews.RemoveRange(reviews);

        if (images.Any())
            _context.CarImages.RemoveRange(images);

        _context.Cars.Remove(car);

        await _context.SaveChangesAsync();
    }
}