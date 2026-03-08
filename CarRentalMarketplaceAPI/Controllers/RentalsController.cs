using CarRentalMarketplaceAPI.DTOs.Rental;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalMarketplaceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRentals()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var rentals = await _rentalService.GetUserRentalsAsync(Guid.Parse(userId));

        return Ok(rentals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRentalDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _rentalService.CreateAsync(Guid.Parse(userId), dto);

        return Ok("Kirayə əməliyyatı uğurla yaradıldı");
    }

    [HttpPut("{rentalId}/complete")]
    public async Task<IActionResult> Complete(Guid rentalId)
    {
        await _rentalService.CompleteAsync(rentalId);

        return Ok("Kirayə tamamlandı");
    }
}