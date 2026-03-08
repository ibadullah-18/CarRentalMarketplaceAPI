using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Rental;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public RentalService(
        IRentalRepository rentalRepository,
        ICarRepository carRepository,
        IMapper mapper)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RentalDto>> GetUserRentalsAsync(Guid userId)
    {
        var rentals = await _rentalRepository.GetUserRentalsAsync(userId);
        return _mapper.Map<IEnumerable<RentalDto>>(rentals);
    }

    public async Task CreateAsync(Guid userId, CreateRentalDto dto)
    {
        var car = await _carRepository.GetByIdAsync(dto.CarId);

        if (car == null)
            throw new Exception("Maşın tapılmadı");

        var totalDays = (dto.EndDate - dto.StartDate).Days;

        if (totalDays <= 0)
            throw new Exception("Tarix aralığı düzgün deyil");

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            CarId = dto.CarId,
            RenterId = userId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            TotalPrice = totalDays * car.PricePerDay,
            Status = RentalStatus.Active,
            CreatedDate = DateTime.UtcNow
        };

        await _rentalRepository.AddAsync(rental);
    }

    public async Task CompleteAsync(Guid rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);

        if (rental == null)
            throw new Exception("Kirayə tapılmadı");

        rental.Status = RentalStatus.Completed;

        await _rentalRepository.UpdateAsync(rental);
    }
}
