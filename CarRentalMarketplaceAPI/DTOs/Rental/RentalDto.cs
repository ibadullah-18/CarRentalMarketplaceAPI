namespace CarRentalMarketplaceAPI.DTOs.Rental;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string CarName { get; set; }
    public string Color { get; set; }
    public decimal PricePerDay { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string PickupLocation { get; set; } = string.Empty;  
    public string ReturnLocation { get; set; } = string.Empty;  
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
}
