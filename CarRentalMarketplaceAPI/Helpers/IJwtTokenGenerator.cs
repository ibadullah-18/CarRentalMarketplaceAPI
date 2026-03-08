using CarRentalMarketplaceAPI.Entities;

namespace CarRentalMarketplaceAPI.Helpers;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
