using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateCarDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createdCar = await _carService.CreateAsync(Guid.Parse(userId!), dto);

        return CreatedAtAction(nameof(GetById), new { id = createdCar.Id }, createdCar);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCarDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        await _carService.UpdateAsync(id, userId, dto);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.DeleteAsync(id, Guid.Parse(userId!));

        return Ok("Maşın deaktiv edildi");
    }

    // bashqa userin masinlari - sadece available olanlar
    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetCarsByOwner(Guid ownerId)
    {
        var cars = await _carService.GetPublicCarsByOwnerIdAsync(ownerId);
        return Ok(cars);
    }

    // menim masinlarim - hamisi
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _carService.AddImageAsync(id, file, isMain, Guid.Parse(userId!));
        return Ok("Şəkil uğurla əlavə olundu");
    }

    [Authorize]
    [HttpDelete("images/{imageId}")]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.DeleteImageAsync(imageId, Guid.Parse(userId!));

        return Ok("Şəkil silindi");
    }

    [Authorize]
    [HttpPut("images/{imageId}/set-main")]
    public async Task<IActionResult> SetMainImage(Guid imageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _carService.SetMainImageAsync(imageId, Guid.Parse(userId!));

        return Ok("Əsas şəkil yeniləndi");
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] CarQueryDto query)
    {
        var cars = await _carService.GetFilteredCarsAsync(query);
        return Ok(cars);
    }

    [Authorize]
    [HttpDelete("{id}/hard-delete")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _carService.HardDeleteAsync(id, Guid.Parse(userId!));
        return Ok("Car permanently deleted");
    }
}