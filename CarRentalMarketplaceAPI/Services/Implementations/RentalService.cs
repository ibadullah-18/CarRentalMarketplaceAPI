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
    private readonly IUserRepository _userRepository;

    public RentalService(
        IRentalRepository rentalRepository,
        ICarRepository carRepository,
        IUserRepository userRepository)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
        _userRepository = userRepository;
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
                Color = car?.Color ?? string.Empty,
                PricePerDay = car?.PricePerDay ?? 0,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                PickupLocation = rental.PickupLocation,
                ReturnLocation = rental.ReturnLocation,
                TotalPrice = rental.TotalPrice,
                Status = rental.Status.ToString()
            });
        }

        return rentalDtos;
    }

    public async Task<IEnumerable<OwnerRentalDto>> GetOwnerRentalsAsync(Guid ownerId)
    {
        var rentals = await _rentalRepository.GetOwnerRentalsAsync(ownerId);
        var result = new List<OwnerRentalDto>();

        foreach (var rental in rentals)
        {
            var car = await _carRepository.GetByIdAsync(rental.CarId);
            var renter = await _userRepository.GetByIdAsync(rental.RenterId);

            result.Add(new OwnerRentalDto
            {
                RentalId = rental.Id,
                CarId = rental.CarId,
                CarName = car != null ? $"{car.Brand} {car.Model}" : "Unknown Car",
                CarBodyType = car?.BodyType.ToString() ?? string.Empty,
                RenterId = rental.RenterId,
                RenterFullName = renter?.FullName ?? "Unknown User",
                RenterEmail = renter?.Email ?? string.Empty,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                PickupLocation = rental.PickupLocation,
                ReturnLocation = rental.ReturnLocation,
                TotalPrice = rental.TotalPrice,
                Status = rental.Status.ToString()
            });
        }

        return result;
    }

    public async Task CreateAsync(Guid userId, CreateRentalDto dto)
    {
        var car = await _carRepository.GetByIdAsync(dto.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (car.OwnerId == userId)
            throw new BadRequestException("Öz maşınınızı kirayə götürə bilməzsiniz");

        if (car.Status == CarStatus.Rented)
            throw new BadRequestException("Bu maşın artıq kirayədədir");

        if (car.Status == CarStatus.Passive)
            throw new BadRequestException("Bu maşın aktiv deyil");

        if (car.Status != CarStatus.Available)
            throw new BadRequestException("Bu maşın kirayə üçün əlçatan deyil");

        if (string.IsNullOrWhiteSpace(dto.PickupLocation))
            throw new BadRequestException("Götürülmə məkanı boş ola bilməz");

        if (string.IsNullOrWhiteSpace(dto.ReturnLocation))
            throw new BadRequestException("Təhvil məkanı boş ola bilməz");

        var totalDays = (dto.EndDate.Date - dto.StartDate.Date).Days;

        if (totalDays <= 0)
            throw new BadRequestException("Tarix aralığı düzgün deyil");

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            CarId = dto.CarId,
            RenterId = userId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            PickupLocation = dto.PickupLocation,
            ReturnLocation = dto.ReturnLocation,
            TotalPrice = totalDays * car.PricePerDay,
            Status = RentalStatus.Active,
            CreatedDate = DateTimeOffset.UtcNow
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

        if (rental.Status != RentalStatus.Active)
            throw new BadRequestException("Yalnız aktiv kirayə tamamlanıla bilər");

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