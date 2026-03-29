using CarRentalMarketplaceAPI.DTOs.Car;

namespace CarRentalMarketplaceAPI.DTOs.Cart;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }

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
    public string MainImageUrl { get; set; } = string.Empty;
    public List<CarImageDto> Images { get; set; } = new();
    public string Location { get; set; } = string.Empty;
    public string BodyType { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
}