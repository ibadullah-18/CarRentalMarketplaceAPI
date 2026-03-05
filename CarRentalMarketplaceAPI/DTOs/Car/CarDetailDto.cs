namespace CarRentalMarketplaceAPI.DTOs.Car;

public class CarDetailDto : CarListDto
{ 
    public int Year { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public List<string> Images { get; set; }
    public string OwnerName { get; set; } = string.Empty;
}
