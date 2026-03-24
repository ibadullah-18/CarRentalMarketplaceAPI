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
    private readonly IUserRepository _userRepository;

    public CarService(
        ICarRepository carRepository,
        ICarImageRepository carImageRepository,
        IMapper mapper,
        IWebHostEnvironment environment,
        IUserRepository userRepository)
    {
        _carRepository = carRepository;
        _carImageRepository = carImageRepository;
        _mapper = mapper;
        _environment = environment;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CarListDto>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        var carDtos = new List<CarListDto>();

        foreach (var car in cars)
        {
            var mainImage = await _carImageRepository.GetMainImageByCarIdAsync(car.Id);
            var images = await _carImageRepository.GetImagesByCarIdAsync(car.Id);

            carDtos.Add(new CarListDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Color = car.Color,
                Mileage = car.Mileage,
                FuelType = car.FuelType,
                Description = car.Description,
                Transmission = car.Transmission,
                PricePerDay = car.PricePerDay,
                Location = car.Location,
                BodyType = car.BodyType.ToString(),
                MainImageUrl = mainImage != null ? $"/{mainImage.ImageUrl}" : null!,
                Images = images.Select(x => new CarImageDto
                {
                    Id = x.Id,
                    ImageUrl = $"/{x.ImageUrl}",
                    IsMain = x.IsMain
                }).ToList()
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

        var owner = await _userRepository.GetByIdAsync(car.OwnerId);

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
            BodyType = car.BodyType.ToString(),
            MainImageUrl = images.FirstOrDefault(x => x.IsMain) != null
                   ? $"/{images.First(x => x.IsMain).ImageUrl}"
                   : null!,
            Images = images.Select(x => new CarImageDto
            {
                Id = x.Id,
                ImageUrl = $"/{x.ImageUrl}",
                IsMain = x.IsMain
            }).ToList(),
            OwnerName = owner != null ? owner.FullName : "Unknown Owner"
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

    public async Task<IEnumerable<OwnerCarsDto>> GetCarsByOwnerAsync(Guid ownerId)
    {
        var cars = await _carRepository.GetCarsByOwnerAsync(ownerId);
        var carDtos = new List<OwnerCarsDto>();

        foreach (var car in cars)
        {
            var mainImage = await _carImageRepository.GetMainImageByCarIdAsync(car.Id);

            carDtos.Add(new OwnerCarsDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                PricePerDay = car.PricePerDay,
                Color = car.Color,
                Location = car.Location,
                BodyType = car.BodyType.ToString(),
                Status = car.Status.ToString(),
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

    public async Task AddImageAsync(Guid carId, IFormFile file, bool isMain, Guid userId)
    {
        var car = await _carRepository.GetByIdAsync(carId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (file == null || file.Length == 0)
            throw new BadRequestException("Şəkil faylı boşdur");

        if (car.OwnerId != userId)
            throw new ForbiddenException("Bu maşına şəkil əlavə etməyə icazəniz yoxdur");

        var relativePath = await FileUploadHelper.SaveFileAsync(file, _environment.WebRootPath, "images/cars");

        if (isMain)
        {
            var existingImages = await _carImageRepository.GetImagesByCarIdAsync(carId);

            foreach (var existingImage in existingImages)
            {
                existingImage.IsMain = false;
                await _carImageRepository.UpdateAsync(existingImage);
            }
        }

        var image = new CarImage
        {
            Id = Guid.NewGuid(),
            CarId = carId,
            ImageUrl = relativePath,
            IsMain = isMain
        };

        await _carImageRepository.AddAsync(image);
    }

    public async Task DeleteImageAsync(Guid imageId, Guid userId)
    {
        var image = await _carImageRepository.GetByIdAsync(imageId);

        if (image == null)
            throw new NotFoundException("Şəkil tapılmadı");

        var car = await _carRepository.GetByIdAsync(image.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (car.OwnerId != userId)
            throw new ForbiddenException("Bu şəkli silməyə icazəniz yoxdur");

        var allImages = (await _carImageRepository.GetImagesByCarIdAsync(car.Id)).ToList();
        var wasMain = image.IsMain;

        await _carImageRepository.DeleteAsync(imageId);

        FileUploadHelper.DeleteFile(_environment.WebRootPath, image.ImageUrl);

        if (wasMain)
        {
            var remainingImages = (await _carImageRepository.GetImagesByCarIdAsync(car.Id)).ToList();

            var firstImage = remainingImages.FirstOrDefault();

            if (firstImage != null)
            {
                firstImage.IsMain = true;
                await _carImageRepository.UpdateAsync(firstImage);
            }
        }
    }

    public async Task SetMainImageAsync(Guid imageId, Guid userId)
    {
        var image = await _carImageRepository.GetByIdAsync(imageId);

        if (image == null)
            throw new NotFoundException("Şəkil tapılmadı");

        var car = await _carRepository.GetByIdAsync(image.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        if (car.OwnerId != userId)
            throw new ForbiddenException("Bu maşının şəklini dəyişməyə icazəniz yoxdur");

        var images = await _carImageRepository.GetImagesByCarIdAsync(car.Id);

        foreach (var item in images)
        {
            item.IsMain = false;
            await _carImageRepository.UpdateAsync(item);
        }

        image.IsMain = true;
        await _carImageRepository.UpdateAsync(image);
    }

    public async Task<IEnumerable<CarListDto>> GetFilteredCarsAsync(CarQueryDto query)
    {
        var cars = await _carRepository.GetFilteredCarsAsync(query);
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
}