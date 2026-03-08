using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public CarService(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CarListDto>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CarListDto>>(cars);
    }

    public async Task<CarDetailDto> GetByIdAsync(Guid id)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new Exception("Maşın tapılmadı");

        return _mapper.Map<CarDetailDto>(car);
    }

    public async Task CreateAsync(Guid ownerId, CreateCarDto dto)
    {
        var car = _mapper.Map<Car>(dto);

        car.Id = Guid.NewGuid();
        car.OwnerId = ownerId;
        car.CreatedDate = DateTime.UtcNow;

        await _carRepository.AddAsync(car);
    }

    public async Task UpdateAsync(Guid id, UpdateCarDto dto)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new Exception("Maşın tapılmadı");

        _mapper.Map(dto, car);

        await _carRepository.UpdateAsync(car);
    }

    public async Task DeleteAsync(Guid id)
    {
        var car = await _carRepository.GetByIdAsync(id);

        if (car == null)
            throw new Exception("Maşın tapılmadı");

        await _carRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CarListDto>> GetCarsByOwnerAsync(Guid ownerId)
    {
        var cars = await _carRepository.GetCarsByOwnerAsync(ownerId);
        return _mapper.Map<IEnumerable<CarListDto>>(cars);
    }
}