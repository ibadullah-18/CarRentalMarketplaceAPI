using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.DTOs.Favorite;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;
    private readonly ICarImageRepository _carImageRepository;

    public FavoriteService(
        ICarImageRepository carImageRepository,
        IFavoriteRepository favoriteRepository,
        ICarRepository carRepository,
        IMapper mapper)
    {
        _favoriteRepository = favoriteRepository;
        _carRepository = carRepository;
        _mapper = mapper;
        _carImageRepository = carImageRepository;
    }

    public async Task<IEnumerable<FavoriteDto>> GetUserFavoritesAsync(Guid userId)
    {
        var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);
        var favoriteDtos = new List<FavoriteDto>();

        foreach (var favorite in favorites)
        {
            var car = await _carRepository.GetByIdAsync(favorite.CarId);

            if (car == null)
                continue;

            var mainImage = await _carImageRepository.GetMainImageByCarIdAsync(car.Id);
            var images = await _carImageRepository.GetImagesByCarIdAsync(car.Id);

            favoriteDtos.Add(new FavoriteDto
            {
                Id = favorite.Id,
                CarId = car.Id,
                OwnerId = car.OwnerId,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Color = car.Color,
                Mileage = car.Mileage,
                FuelType = car.FuelType.ToString(),
                Transmission = car.Transmission.ToString(),
                Description = car.Description,
                PricePerDay = car.PricePerDay,
                Location = car.Location,
                BodyType = car.BodyType.ToString(),
                MainImageUrl = mainImage != null ? $"/{mainImage.ImageUrl}" : null!,
                Images = images.Select(x => new CarImageDto
                {
                    Id = x.Id,
                    ImageUrl = $"/{x.ImageUrl}",
                    IsMain = x.IsMain
                }).ToList(),
                CreatedDate = favorite.CreatedDate
            });
        }

        return favoriteDtos;
    }


    public async Task AddAsync(Guid userId, AddFavoriteDto dto)
    {
        var car = await _carRepository.GetByIdAsync(dto.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        var favorite = new Favorite
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CarId = dto.CarId,
            CreatedDate = DateTime.UtcNow
        };

        await _favoriteRepository.AddAsync(favorite);
    }

    public async Task RemoveAsync(Guid userId, Guid carId)
    {
        await _favoriteRepository.RemoveAsync(userId, carId);
    }
}