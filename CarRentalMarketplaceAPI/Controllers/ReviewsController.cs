using CarRentalMarketplaceAPI.DTOs.Review;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalMarketplaceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("car/{carId}")]
    public async Task<IActionResult> GetCarReviews(Guid carId)
    {
        var reviews = await _reviewService.GetCarReviewsAsync(carId);

        return Ok(reviews);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateReviewDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _reviewService.AddAsync(Guid.Parse(userId), dto);

        return Ok("Rəy uğurla əlavə olundu");
    }

    [Authorize]
    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> Delete(Guid reviewId)
    {
        await _reviewService.DeleteAsync(reviewId);

        return Ok("Rəy silindi");
    }
}