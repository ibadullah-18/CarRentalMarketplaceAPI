using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalMarketplaceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserDto dto)
    {
        await _userService.UpdateAsync(id, dto);
        return Ok("İstifadəçi məlumatları yeniləndi");
    }
}