namespace CarRentalMarketplaceAPI.DTOs.Car;

public class CarListDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }

    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerDay { get; set; }        
    public string MainImageUrl { get; set; }
    public List<CarImageDto> Images { get; set; }   
    public string Location { get; set; } = string.Empty;
    public string BodyType { get; set; } = string.Empty;
}
