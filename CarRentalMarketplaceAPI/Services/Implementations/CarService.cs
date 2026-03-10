using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Enums;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using CarRentalMarketplaceAPI.Helpers;
using Microsoft.AspNetCore.Http;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly ICarImageRepository _carImageRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public CarService(
        ICarRepository carRepository,
        ICarImageRepository carImageRepository,
        IMapper mapper,
        IWebHostEnvironment environment)
    {
        _carRepository = carRepository;
        _carImageRepository = carImageRepository;
        _mapper = mapper;
        _environment = environment;
    }

    public async Task<IEnumerable<CarListDto>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        var carDtos = new List<CarListDto>();

        foreach (var car in cars)
        {
            var mainImage = await _carImageRepository.GetMainImageByCarIdAsync(car.Id);

            carDtos.Add(new CarListDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                Location = car.Location,
                MainImageUrl = mainImage != null ? $"/{mainImage.ImageUrl}" : null!
            });
        }

        return carDtos;
    }

    public async Task<CarDetailDto> GetByIdAsync(Guid id)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        var images = await _carImageRepository.GetImagesByCarIdAsync(id);

        var dto = new CarDetailDto
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            PricePerDay = car.PricePerDay,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            Mileage = car.Mileage,
            Description = car.Description,
            Location = car.Location,
            Color = car.Color,
            MainImageUrl = images.FirstOrDefault(x => x.IsMain) != null
                ? $"/{images.First(x => x.IsMain).ImageUrl}"
                : null!,
            Images = images.Select(x => $"/{x.ImageUrl}").ToList()
        };

        return dto;
    }

    public async Task CreateAsync(Guid ownerId, CreateCarDto dto)
    {
        var car = _mapper.Map<Car>(dto);

        car.Id = Guid.NewGuid();
        car.OwnerId = ownerId;
        car.Status = CarStatus.Available;
        car.CreatedDate = DateTime.UtcNow;

        await _carRepository.AddAsync(car);
    }

    public async Task UpdateAsync(Guid id, Guid userId, UpdateCarDto dto)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (car.OwnerId != userId)
            throw new ForbiddenException("Bu maşını yeniləməyə icazəniz yoxdur");

        _mapper.Map(dto, car);

        await _carRepository.UpdateAsync(car);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (car.OwnerId != userId)
            throw new ForbiddenException("Bu maşını deaktiv etməyə icazəniz yoxdur");

        await _carRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CarListDto>> GetCarsByOwnerAsync(Guid ownerId)
    {
        var cars = await _carRepository.GetCarsByOwnerAsync(ownerId);
        var carDtos = new List<CarListDto>();

        foreach (var car in cars)
        {
            var mainImage = await _carImageRepository.GetMainImageByCarIdAsync(car.Id);

            carDtos.Add(new CarListDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                Location = car.Location,
                MainImageUrl = mainImage != null ? $"/{mainImage.ImageUrl}" : null
            });
        }

        return carDtos;
    }

    public async Task ActivateAsync(Guid id, Guid userId)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (car.OwnerId != userId)
            throw new ForbiddenException("Bu maşını aktiv etməyə icazəniz yoxdur");

        car.Status = CarStatus.Available;

        await _carRepository.UpdateAsync(car);
    }

    public async Task AddImageAsync(Guid carId, IFormFile file, bool isMain)
    {
        var car = await _carRepository.GetByIdAsync(carId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (file == null || file.Length == 0)
            throw new BadRequestException("Şəkil faylı boşdur");

        var relativePath = await FileUploadHelper.SaveFileAsync(file, _environment.WebRootPath, "images/cars");

        var image = new CarImage
        {
            Id = Guid.NewGuid(),
            CarId = carId,
            ImageUrl = relativePath,
            IsMain = isMain
        };

        await _carImageRepository.AddAsync(image);
    }
}