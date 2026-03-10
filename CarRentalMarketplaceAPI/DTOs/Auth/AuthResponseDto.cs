using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalMarketplaceAPI.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public UserDto User { get; set; }   
}
