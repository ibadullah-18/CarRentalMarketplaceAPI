using CarRentalMarketplaceAPI.DTOs.User;

namespace CarRentalMarketplaceAPI.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; }   
}
