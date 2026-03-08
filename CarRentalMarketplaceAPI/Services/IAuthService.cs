using CarRentalMarketplaceAPI.DTOs.Auth;

namespace CarRentalMarketplaceAPI.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
