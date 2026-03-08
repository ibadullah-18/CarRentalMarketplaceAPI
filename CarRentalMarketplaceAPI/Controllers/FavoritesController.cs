using CarRentalMarketplaceAPI.DTOs.Favorite;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalMarketplaceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserFavorites()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var favorites = await _favoriteService.GetUserFavoritesAsync(Guid.Parse(userId));

        return Ok(favorites);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddFavoriteDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _favoriteService.AddAsync(Guid.Parse(userId), dto);

        return Ok("Maşın favorilərə əlavə olundu");
    }

    [HttpDelete("{carId}")]
    public async Task<IActionResult> Remove(Guid carId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _favoriteService.RemoveAsync(Guid.Parse(userId), carId);

        return Ok("Maşın favorilərdən silindi");
    }
}