namespace CarRentalMarketplaceAPI.DTOs.User;

public class UpdateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string DriverLicenseNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? CurrentPassword { get; set; }  
    public string? NewPassword { get; set; }       

    public IFormFile? ProfileImage { get; set; }
}