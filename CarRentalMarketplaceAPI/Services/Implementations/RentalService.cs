using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Rental;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Enums;
using CarRentalMarketplaceAPI.Exceptions;
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

        var rentalDtos = new List<RentalDto>();

        foreach (var rental in rentals)
        {
            var car = await _carRepository.GetByIdAsync(rental.CarId);

            rentalDtos.Add(new RentalDto
            {
                Id = rental.Id,
                CarId = rental.CarId,
                CarName = car != null ? $"{car.Brand} {car.Model}" : "Unknown Car",
                Color = car?.Color!,
                PricePerDay = car?.PricePerDay ?? 0,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                TotalPrice = rental.TotalPrice,
                Status = rental.Status.ToString()
            });
        }

        return rentalDtos;
    }

    public async Task CreateAsync(Guid userId, CreateRentalDto dto)
    {
        var car = await _carRepository.GetByIdAsync(dto.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        var totalDays = (dto.EndDate - dto.StartDate).Days;

        if (totalDays <= 0)
            throw new BadRequestException("Tarix aralığı düzgün deyil");

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

        car.Status = CarStatus.Rented;
        await _carRepository.UpdateAsync(car);
    }

    public async Task CompleteAsync(Guid rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);

        if (rental == null)
            throw new NotFoundException("Kirayə tapılmadı");

        rental.Status = RentalStatus.Completed;
        await _rentalRepository.UpdateAsync(rental);

        var car = await _carRepository.GetByIdAsync(rental.CarId);

        if (car != null)
        {
            car.Status = CarStatus.Available;
            await _carRepository.UpdateAsync(car);
        }
    }
}
