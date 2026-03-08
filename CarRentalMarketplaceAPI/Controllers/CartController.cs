using CarRentalMarketplaceAPI.DTOs.Cart;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalMarketplaceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var cart = await _cartService.GetCartAsync(Guid.Parse(userId));

        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _cartService.AddItemAsync(Guid.Parse(userId), dto);

        return Ok("Maşın səbətə əlavə olundu");
    }

    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        await _cartService.RemoveItemAsync(itemId);

        return Ok("Maşın səbətdən silindi");
    }
}