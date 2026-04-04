namespace CarRentalMarketplaceAPI.DTOs.User;

public class UpdateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string DriverLicenseNumber { get; set; } = string.Empty;
    public IFormFile? ProfileImage { get; set; }
}
