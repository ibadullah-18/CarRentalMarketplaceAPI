namespace CarRentalMarketplaceAPI.DTOs.Car;

public class OwnerCarsDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal PricePerDay { get; set; }
    public string Color { get; set; }
    public string Location { get; set; }
    public string BodyType { get; set; } = string.Empty;
    public string Status { get; set; }
    public string? MainImageUrl { get; set; }
}
