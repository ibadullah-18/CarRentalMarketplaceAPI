using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        await _carService.CreateAsync(Guid.Parse(userId), dto);

        return Ok("Maşın uğurla əlavə olundu");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCarDto dto)
    {
        await _carService.UpdateAsync(id, dto);
        return Ok("Maşın məlumatları yeniləndi");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _carService.DeleteAsync(id);
        return Ok("Maşın silindi");
    }

    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetCarsByOwner(Guid ownerId)
    {
        var cars = await _carService.GetCarsByOwnerAsync(ownerId);
        return Ok(cars);
    }
}
