namespace CarRentalMarketplaceAPI.Entities;

public class Car
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; } 
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Year { get; set; } 
    public decimal PricePerDay { get; set; } 
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
}
