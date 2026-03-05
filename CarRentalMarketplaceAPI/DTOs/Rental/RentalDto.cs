namespace CarRentalMarketplaceAPI.DTOs.Rental;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}
