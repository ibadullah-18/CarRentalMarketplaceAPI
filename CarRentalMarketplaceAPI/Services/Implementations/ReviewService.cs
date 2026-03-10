using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Review;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public ReviewService(
        IReviewRepository reviewRepository,
        ICarRepository carRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReviewDto>> GetCarReviewsAsync(Guid carId)
    {
        var reviews = await _reviewRepository.GetCarReviewsAsync(carId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task AddAsync(Guid userId, CreateReviewDto dto)
    {
        var car = await _carRepository.GetByIdAsync(dto.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CarId = dto.CarId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedDate = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);
    }

    public async Task DeleteAsync(Guid reviewId)
    {
        await _reviewRepository.DeleteAsync(reviewId);
    }
}
