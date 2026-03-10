namespace CarRentalMarketplaceAPI.DTOs.Car;

public class CarImageDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public bool IsMain { get; set; }
}