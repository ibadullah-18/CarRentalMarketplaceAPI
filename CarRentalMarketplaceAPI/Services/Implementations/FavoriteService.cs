using AutoMapper;
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

    public FavoriteService(
        IFavoriteRepository favoriteRepository,
        ICarRepository carRepository,
        IMapper mapper)
    {
        _favoriteRepository = favoriteRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FavoriteDto>> GetUserFavoritesAsync(Guid userId)
    {
        var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);
        return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
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