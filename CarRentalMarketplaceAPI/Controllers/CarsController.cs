using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarRentalMarketplaceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carService.GetAllAsync();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var car = await _carService.GetByIdAsync(id);
        return Ok(car);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCarDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.CreateAsync(Guid.Parse(userId!), dto);

        return Ok("Maşın uğurla əlavə olundu");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCarDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.UpdateAsync(id, Guid.Parse(userId!), dto);

        return Ok("Maşın məlumatları yeniləndi");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.DeleteAsync(id, Guid.Parse(userId!));

        return Ok("Maşın deaktiv edildi");
    }

    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetCarsByOwner(Guid ownerId)
    {
        var cars = await _carService.GetCarsByOwnerAsync(ownerId);
        return Ok(cars);
    }

    [Authorize]
    [HttpGet("my-cars")]
    public async Task<IActionResult> GetMyCars()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var cars = await _carService.GetCarsByOwnerAsync(Guid.Parse(userId!));

        return Ok(cars);
    }

    [Authorize]
    [HttpPut("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.ActivateAsync(id, Guid.Parse(userId!));

        return Ok("Maşın yenidən aktiv edildi");
    }

    [Authorize]
    [HttpPost("{id}/images")]
    public async Task<IActionResult> AddImage(Guid id, IFormFile file, [FromQuery] bool isMain = false)
    {
        await _carService.AddImageAsync(id, file, isMain);
        return Ok("Şəkil uğurla əlavə olundu");
    }
}