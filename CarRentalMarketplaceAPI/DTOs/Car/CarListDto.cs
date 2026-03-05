namespace CarRentalMarketplaceAPI.DTOs.Car;

public class CarListDto
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public decimal PricePerDay { get; set; }        
    public string MainImageUrl { get; set; } 
    public string Location { get; set; } = string.Empty;
}
